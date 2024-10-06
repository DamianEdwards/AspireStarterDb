using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspireStarterDb.ApiDbService.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Todos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CompletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Todos", x => x.Id);
                    table.CheckConstraint("CK_CompletedOn", "[CompletedOn] = NULL OR [CompletedOn] > [CreatedOn]");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Todos_CreatedOn",
                table: "Todos",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Todos_Name",
                table: "Todos",
                columns: new[] { "Title", "CompletedOn" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Todos");
        }
    }
}
