#addin nuget:?package=Cake.Docker&version=1.2.0

var target = Argument("target", "Default");

var useCache = false;

var migrationsProjectDirectory = "../src/NetTransactions.Migrations";
var localSqlConnection = "Server=localhost;Database=transactions;Port=5432;User Id=desenv;Password=P@ssword123";

var apiDirectory = "../src/NetTransactions.Api";
var migrationsDirectory = "../src/NetTransactions.Migrations";
var tagDockerNetTransactionsApi = new string[] { "transactions/transactions-api:latest"};
var tagDockerNetTransactionsMigration = new string[] { "transactions/transactions-migration:latest"};

Task("BuildApiImage")
.Does(() => {
    Information($"Generating API image with version: {string.Join(",", tagDockerNetTransactionsApi)}");
    DockerBuild(new DockerImageBuildSettings { File = $"{apiDirectory}/Dockerfile", Tag = tagDockerNetTransactionsApi, NoCache=!useCache }, "../");
    Information($"Image generated: {string.Join(",", tagDockerNetTransactionsApi)}. Loading on Kind");
    using(var process = StartAndReturnProcess("kind", new ProcessSettings{ Arguments = $"load docker-image {tagDockerNetTransactionsApi[0]}" }))
        process.WaitForExit();
});

Task("BuildMigrationsImage")
.Does(() => {
    Information($"Generating Migrations image with version: {string.Join(",", tagDockerNetTransactionsMigration)}");
    DockerBuild(new DockerImageBuildSettings { File = $"{migrationsDirectory}/Dockerfile", Tag = tagDockerNetTransactionsMigration, NoCache=!useCache }, "../");
    Information($"Image generated: {string.Join(",", tagDockerNetTransactionsMigration)}. Loading on Kind");
    using(var process = StartAndReturnProcess("kind", new ProcessSettings{ Arguments = $"load docker-image {tagDockerNetTransactionsMigration[0]}" }))
        process.WaitForExit();
});

Task("ExecuteMigrations")
.Does(() => {
    Information($"Executing migrations for connection: {localSqlConnection}");

    using(var process = StartAndReturnProcess("dotnet", new ProcessSettings{ Arguments = $"run up -s \"{localSqlConnection}\"", WorkingDirectory = migrationsProjectDirectory }))
        process.WaitForExit();

    Information($"Migrations executed successfully.");
});

Task("ExecuteMigrationsUp")
.Does(() => {
    Information($"Executing migrations up for connection: {localSqlConnection}");

    using(var process = StartAndReturnProcess("dotnet", new ProcessSettings{ Arguments = $"run up -s \"{localSqlConnection}\"", WorkingDirectory = migrationsProjectDirectory }))
        process.WaitForExit();

    Information($"Migrations executed successfully.");
});

Task("ExecuteMigrationsDown")
.Does(() => {
    Information($"Executing migrations down for connection: {localSqlConnection}");

    using(var process = StartAndReturnProcess("dotnet", new ProcessSettings{ Arguments = $"run down -s \"{localSqlConnection}\"", WorkingDirectory = migrationsProjectDirectory }))
        process.WaitForExit();

    Information($"Migrations executed successfully.");
});

RunTarget(target);