using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MB.Access.Tenant.Database.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "tenant");

            migrationBuilder.CreateTable(
                name: "Message",
                schema: "tenant",
                columns: table => new
                {
                    TenantId = table.Column<int>(nullable: false),
                    MessageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Subject = table.Column<string>(maxLength: 250, nullable: false),
                    Body = table.Column<string>(nullable: false),
                    PublishedOnUTC = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => new { x.TenantId, x.MessageId });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Message",
                schema: "tenant");
        }
    }
}
