using Dapper;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Exceptions;
using FluentMigrator.Runner.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Reflection;

namespace NetTransactions.Migrations;
public class MigrationService
{
    private readonly string _connectionString;

    public MigrationService(string connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException(nameof(connectionString));

        _connectionString = connectionString;
    }

    private void CreateDatabaseIfItDoesNotExist()
    {
        var connectionStringBuilder = new NpgsqlConnectionStringBuilder(_connectionString);

        var database = connectionStringBuilder.Database;

        connectionStringBuilder.Database = "template1";

        using var connection = new NpgsqlConnection(connectionStringBuilder.ToString());

        var databaseAlreadyExists = connection.QuerySingle<bool>($@"SELECT EXISTS (SELECT datname FROM pg_catalog.pg_database WHERE datname = '{database}')");

        if (!databaseAlreadyExists)
            connection.Execute($@"CREATE DATABASE {database};");
    }

    public void Up(Assembly assembly, bool shouldGenerateScript) =>
        Execute((executor) => executor.MigrateUp(), assembly, shouldGenerateScript);

    public void Up(long version, Assembly assembly, bool shouldGenerateScript) =>
        Execute((executor) => executor.MigrateUp(version), assembly, shouldGenerateScript);

    public void Down(long version, Assembly assembly, bool shouldGenerateScript) =>
        Execute((executor) => executor.MigrateDown(version), assembly, shouldGenerateScript);

    private void Execute(Action<IMigrationRunner> action, Assembly assembly, bool shouldGenerateScript)
    {
        CreateDatabaseIfItDoesNotExist();

        var serviceProvider = new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(
                runnerBuilder =>
                {
                    runnerBuilder
                        .AddPostgres()
                        .WithGlobalConnectionString(_connectionString)
                        .ScanIn(assembly).For.Migrations();

                    if (shouldGenerateScript)
                        runnerBuilder.AsGlobalPreview();
                })
            .AddLogging(lb => lb.AddFluentMigratorConsole());

        if (shouldGenerateScript)
        {
            serviceProvider = serviceProvider
                .AddSingleton<ILoggerProvider, LogFileFluentMigratorLoggerProvider>()
                .Configure<LogFileFluentMigratorLoggerOptions>(
                    opt =>
                    {
                        opt.OutputFileName = "migrationsScript.sql";
                        opt.OutputGoBetweenStatements = false;
                        opt.ShowSql = true;
                    });
        }

        var executor = serviceProvider.BuildServiceProvider().GetRequiredService<IMigrationRunner>();
        action(executor);
    }

    public static async Task<int> ExecuteMigration(string[] args, Assembly assembly)
    {
        var connectionStringOption = new Option<string>(new[] { "--connectionString", "-s" }, "Database connection string")
        {
            IsRequired = true,
            Arity = ArgumentArity.ExactlyOne
        };

        var incrementVersionOption = new Option<long?>(new[] { "--version-up", "-u" }, "Requested version")
        {
            IsRequired = false,
            Arity = ArgumentArity.ZeroOrOne
        };

        var decreaseVersionOption = new Option<long>(new[] { "--version-down", "-d" }, "Requested version")
        {
            IsRequired = false,
            Arity = ArgumentArity.ExactlyOne
        };

        var scriptOption = new Option<bool>("--script", "Choose if the migration SQL script should be generated in a file.")
        {
            IsRequired = false,
            Arity = ArgumentArity.Zero
        };

        var commandUp = new Command("up", "Migrations up");

        commandUp.SetHandler<string, long?, bool, InvocationContext>((connectionString, version, shouldGenerateScript, ctx) =>
        {
            try
            {
                var migrationService = new MigrationService(connectionString);

                if (version.HasValue)
                    migrationService.Up(version.Value, assembly, shouldGenerateScript);
                else
                    migrationService.Up(assembly, shouldGenerateScript);
            }
            catch (MissingMigrationsException)
            {
                ctx.ExitCode = 64;
                return;
            }
            ctx.ExitCode = 0;
        }, connectionStringOption, incrementVersionOption, scriptOption);

        commandUp.AddOption(incrementVersionOption);
        commandUp.AddOption(connectionStringOption);
        commandUp.AddOption(scriptOption);

        var commandDown = new Command("down", "Migrations down");

        commandDown.SetHandler<string, long, bool, InvocationContext>((connectionString, version, shouldGenerateScript, ctx) =>
        {
            try
            {
                new MigrationService(connectionString).Down(version, assembly, shouldGenerateScript);
            }
            catch (MissingMigrationsException)
            {
                ctx.ExitCode = 64;
                return;
            }
            ctx.ExitCode = 0;
        }, connectionStringOption, decreaseVersionOption, scriptOption);

        commandDown.AddOption(decreaseVersionOption);
        commandDown.AddOption(connectionStringOption);
        commandDown.AddOption(scriptOption);

        var migrationCommandBuilder = new CommandLineBuilder(new RootCommand("Command to execute migrations")).UseDefaults();

        migrationCommandBuilder.Command.AddCommand(commandUp);
        migrationCommandBuilder.Command.AddCommand(commandDown);

        var migrationCommand = migrationCommandBuilder.Build();

        var exitCode = await migrationCommand.InvokeAsync(args);
        return exitCode;
    }
}
