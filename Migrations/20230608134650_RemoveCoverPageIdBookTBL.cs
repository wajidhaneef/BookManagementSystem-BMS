using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookManagementSystem_BMS.Migrations
{
    public partial class RemoveCoverPageIdBookTBL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_CoverPages_CoverPageId",
                schema: "BMS",
                table: "Book");

            migrationBuilder.DropIndex(
                name: "IX_Book_CoverPageId",
                schema: "BMS",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "CoverPageId",
                schema: "BMS",
                table: "Book");

            migrationBuilder.CreateIndex(
                name: "IX_CoverPages_BookId",
                table: "CoverPages",
                column: "BookId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CoverPages_Book_BookId",
                table: "CoverPages",
                column: "BookId",
                principalSchema: "BMS",
                principalTable: "Book",
                principalColumn: "BookID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoverPages_Book_BookId",
                table: "CoverPages");

            migrationBuilder.DropIndex(
                name: "IX_CoverPages_BookId",
                table: "CoverPages");

            migrationBuilder.AddColumn<int>(
                name: "CoverPageId",
                schema: "BMS",
                table: "Book",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
    }
}
