using Microsoft.EntityFrameworkCore.Migrations;
namespace Domain.Utils;

public static class ModelMigrationExtensions {
    public static void CreateTableTriggers(this MigrationBuilder migrationBuilder, List<string> tables) {
        foreach (var tableName in tables) {
            // Create trigger script for UUID()
            var triggerName = $"generate_uuid_before_insert_{tableName}";
            var createTriggerSql = $@"
            CREATE TRIGGER `{triggerName}` 
            BEFORE INSERT ON `{tableName}`
            FOR EACH ROW 
            BEGIN
                IF NEW.`Id` IS NULL THEN
                    SET NEW.`Id` = UUID(); 
                END IF;
            END";

            migrationBuilder.Sql(createTriggerSql);
        }
    }
    public static void DropTableTriggers(this MigrationBuilder migrationBuilder, List<string> tables) {
        foreach (var tableName in tables) {
            // Create trigger script for UUID()
            var triggerName = $"generate_uuid_before_insert_{tableName}";
            var dropTriggerSql = $@"
            DROP TRIGGER IF EXISTS `{triggerName}`";

            migrationBuilder.Sql(dropTriggerSql);
        }

    }
}