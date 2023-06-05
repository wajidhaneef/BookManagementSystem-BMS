using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookManagementSystem_BMS.Migrations
{
    public partial class addCoverPageTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CoverPageId",
                schema: "BMS",
                table: "Book",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CoverPages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageData = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    ImageMimeType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoverPages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Book_CoverPageId",
                schema: "BMS",
                table: "Book",
                column: "CoverPageId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Book_CoverPages_CoverPageId",
                schema: "BMS",
                table: "Book",
                column: "CoverPageId",
                principalTable: "CoverPages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_CoverPages_CoverPageId",
                schema: "BMS",
                table: "Book");

            migrationBuilder.DropTable(
                name: "CoverPages");

            migrationBuilder.DropIndex(
                name: "IX_Book_CoverPageId",
                schema: "BMS",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "CoverPageId",
                schema: "BMS",
                table: "Book");
        }
    }
}
