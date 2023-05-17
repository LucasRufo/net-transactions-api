using NetTransactions.Migrations;
using NetTransactions.Migrations.Migrations;

await MigrationService.ExecuteMigration(args, typeof(CreateParticipantTable).Assembly);