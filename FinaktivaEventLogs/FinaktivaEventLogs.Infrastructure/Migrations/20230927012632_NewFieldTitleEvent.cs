using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinaktivaEventLogs.Infrastructure.Migrations
{
    public partial class NewFieldTitleEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "EventLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "EventLogs");
        }
    }
}
