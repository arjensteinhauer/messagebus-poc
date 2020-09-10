using MB.Utilities.Extensions;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MB.Access.Tenant.Database.Migrations
{
    public partial class AddTenantAccessPolicyForMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TenantName",
                schema: "tenant",
                table: "Message",
                maxLength: 100,
                nullable: false,
                defaultValueSql: "CAST(SESSION_CONTEXT(N'TenantName') AS nvarchar(100))",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddTenantAccessSecuritySupport("tenant.Message");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RemoveTenantAccessSecuritySupport("tenant.Message");

            migrationBuilder.AlterColumn<string>(
                name: "TenantName",
                schema: "tenant",
                table: "Message",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldDefaultValueSql: "CAST(SESSION_CONTEXT(N'TenantName') AS nvarchar(100))");
        }
    }
}
