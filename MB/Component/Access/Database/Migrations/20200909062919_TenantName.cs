using Microsoft.EntityFrameworkCore.Migrations;

namespace MB.Access.Tenant.Database.Migrations
{
    public partial class TenantName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Message",
                schema: "tenant",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "tenant",
                table: "Message");

            migrationBuilder.AddColumn<string>(
                name: "TenantName",
                schema: "tenant",
                table: "Message",
                maxLength: 100,
                nullable: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Message",
                schema: "tenant",
                table: "Message",
                columns: new[] { "TenantName", "MessageId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Message",
                schema: "tenant",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "TenantName",
                schema: "tenant",
                table: "Message");

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                schema: "tenant",
                table: "Message",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Message",
                schema: "tenant",
                table: "Message",
                columns: new[] { "TenantId", "MessageId" });
        }
    }
}
