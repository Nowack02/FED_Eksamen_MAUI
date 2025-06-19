using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamAppMAUI.Migrations
{
    /// <inheritdoc />
    public partial class changesToJoinTabelOnceAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "StudentExams",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "StudentExams");
        }
    }
}
