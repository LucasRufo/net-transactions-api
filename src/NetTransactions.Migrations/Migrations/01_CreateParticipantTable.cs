using FluentMigrator;

namespace NetTransactions.Migrations.Migrations;

[Migration(1)]
public class CreateParticipantTable : Migration
{
    const string tableName = "participant";
    public override void Up()
    {
        Create.Table(tableName)
            .WithColumn("id").AsGuid().NotNullable().PrimaryKey()
            .WithColumn("name").AsString(200).NotNullable()
            .WithColumn("email").AsString(200).NotNullable()
            .WithColumn("cpf").AsString(11).NotNullable()
            .WithColumn("created_at").AsDateTime().NotNullable()
            .WithColumn("updated_at").AsDateTime().Nullable()
            .WithColumn("deleted_at").AsDateTime().Nullable();
    }

    public override void Down() => Delete.Table(tableName);
}
