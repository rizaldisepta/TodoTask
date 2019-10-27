using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDoListApp.Data.Migrations
{
    public partial class AddTasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
            name: "Tasks",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                DateModified = table.Column<DateTime>(nullable: true),
                IsDone = table.Column<bool>(nullable: false),
                Title = table.Column<string>(nullable: true),
                Details = table.Column<string>(nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Items", x => x.Id);
            });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
            name: "Tasks");
        }
    }
}
