using Microsoft.EntityFrameworkCore.Migrations;

namespace MB.Utilities.Extensions
{
    public static class TemporalTablesExtension
    {
        public static void AddTemporalTableSupport(this MigrationBuilder builder, string tableNameWithSchema, string historyTableNameWithSchema)
        {
            string constraintNamesPrefix = tableNameWithSchema.Replace(".", "_");

            builder.Sql(
                $@"ALTER TABLE {tableNameWithSchema} ADD 
                   StartDateTime datetime2(0) GENERATED ALWAYS AS ROW START HIDDEN NOT NULL CONSTRAINT {constraintNamesPrefix}_DefaultSysDate DEFAULT SYSUTCDATETIME(),
                   EndDateTime datetime2(0) GENERATED ALWAYS AS ROW END HIDDEN NOT NULL CONSTRAINT {constraintNamesPrefix}_DefaultEndDate DEFAULT CAST('9999-12-31 23:59:59.9999999' AS datetime2),
                   PERIOD FOR SYSTEM_TIME (StartDateTime, EndDateTime);");
            builder.Sql(
                $@"ALTER TABLE {tableNameWithSchema} 
                   SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE = {historyTableNameWithSchema} ));");
        }

        public static void RemoveTemporalTableSupport(this MigrationBuilder builder, string tableNameWithSchema, string historyTableNameWithSchema)
        {
            string constraintNamesPrefix = tableNameWithSchema.Replace(".", "_");

            builder.Sql($@"ALTER TABLE {tableNameWithSchema} SET(SYSTEM_VERSIONING = OFF);");
            builder.Sql($@"ALTER TABLE {tableNameWithSchema} DROP PERIOD FOR SYSTEM_TIME;");
            builder.Sql($@"ALTER TABLE {tableNameWithSchema} DROP CONSTRAINT {constraintNamesPrefix}_DefaultEndDate;");
            builder.Sql($@"ALTER TABLE {tableNameWithSchema} DROP CONSTRAINT {constraintNamesPrefix}_DefaultSysDate;");
            builder.Sql($@"ALTER TABLE {tableNameWithSchema} DROP COLUMN EndDateTime;");
            builder.Sql($@"ALTER TABLE {tableNameWithSchema} DROP COLUMN StartDateTime;");

            builder.Sql($@"DROP TABLE {historyTableNameWithSchema};");
        }
    }
}
