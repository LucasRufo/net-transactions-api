using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            .WithColumn("document").AsString(30).NotNullable()
            .WithColumn("created_at").AsDateTime().NotNullable()
            .WithColumn("updated_at").AsDateTime().Nullable();
    }

    public override void Down() => Delete.Table(tableName);
}
