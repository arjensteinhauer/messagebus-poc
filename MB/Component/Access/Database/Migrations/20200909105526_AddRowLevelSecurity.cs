using Microsoft.EntityFrameworkCore.Migrations;

namespace MB.Access.Tenant.Database.Migrations
{
    public partial class AddRowLevelSecurity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(name: "security");

            migrationBuilder.Sql(
                $@"CREATE FUNCTION security.fn_tenantAccessPredicate(@tenantName nvarchar(100))
                       RETURNS TABLE
                       WITH SCHEMABINDING
                   AS
                       RETURN SELECT 1 AS fn_tenantAccessResult
                       WHERE
                       (
                           DATABASE_PRINCIPAL_ID() = DATABASE_PRINCIPAL_ID('tenantAccessUser')
                           AND CAST(SESSION_CONTEXT(N'TenantName') AS nvarchar(100)) = @tenantName
                       )
                       OR
                       (
                           DATABASE_PRINCIPAL_ID() = DATABASE_PRINCIPAL_ID('dbo')
                       )");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                $@"DROP FUNCTION security.fn_tenantAccessPredicate;");

            migrationBuilder.DropSchema("security");
        }
    }
}
