using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JrsExpressAccounting.Web.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    Action = table.Column<string>(maxLength: 50, nullable: false),
                    TableName = table.Column<string>(maxLength: 120, nullable: false),
                    RecordId = table.Column<string>(maxLength: 50, nullable: false),
                    UserName = table.Column<string>(maxLength: 100, nullable: false),
                    ActionTimeUtc = table.Column<DateTime>(nullable: false),
                    Changes = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_AuditLogs", x => x.Id); });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "AuditLogs");
        }
    }
}
