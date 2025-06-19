using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamAppMAUI.Migrations
{
    /// <inheritdoc />
    public partial class JoinTabelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "StudentExams",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "StudentExams");
        }
    }
}
