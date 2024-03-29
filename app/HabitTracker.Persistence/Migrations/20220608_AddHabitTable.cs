using FluentMigrator;
using FluentMigrator.Builders;
using FluentMigrator.Builders.Create.Table;

namespace HabitTracker.Persistence.Migrations;

[Migration(202206082108)]
public class AddHabitTable : Migration
{
    public override void Up()
    {
        ICreateTableColumnOptionOrWithColumnSyntax _ = Create.Table("Habit")
            .WithColumn("Id").AsString(36).NotNullable().PrimaryKey()
            .WithColumn("Title").AsString(60).NotNullable()
            .WithColumn("Description").AsString().Nullable()
            .WithColumn("CreatedAt").AsDateTime().NotNullable()
            .WithColumn("ModifiedAt").AsDateTime().NotNullable();
    }

    public override void Down()
    {
        IInSchemaSyntax _ = Delete.Table("Habit");
    }
}
