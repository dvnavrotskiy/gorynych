using FluentMigrator;

namespace gorynych.api.Dal.Migrations;

[Migration(202504301104)]
public class CreateMessagesTable : Migration
{
    public override void Up()
    {
        Execute.Sql(@"
CREATE TABLE IF NOT EXISTS Messages (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Message TEXT NOT NULL,
    Timestamp DATETIME NOT NULL    
)
");
    }

    public override void Down()
    {
        Execute.Sql(@"
DROP TABLE IF EXISTS Messages
");
    }
}