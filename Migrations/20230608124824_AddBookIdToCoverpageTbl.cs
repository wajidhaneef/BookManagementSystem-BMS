using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookManagementSystem_BMS.Migrations
{
    public partial class AddBookIdToCoverpageTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "CoverPages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "CoverPages",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookId",
                table: "CoverPages");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "CoverPages");
        }
    }
}
