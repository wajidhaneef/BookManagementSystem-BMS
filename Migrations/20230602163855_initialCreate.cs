using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookManagementSystem_BMS.Migrations
{
    public partial class initialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "BMS");

            migrationBuilder.CreateTable(
                name: "Category",
                schema: "BMS",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "BMS",
                columns: table => new
                {
                    RoleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleID);
                });

            migrationBuilder.CreateTable(
                name: "Book",
                schema: "BMS",
                columns: table => new
                {
                    BookID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book", x => x.BookID);
                    table.ForeignKey(
                        name: "FK_Book_Category_CategoryID",
                        column: x => x.CategoryID,
                        principalSchema: "BMS",
                        principalTable: "Category",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Roles_Permissions",
                schema: "BMS",
                columns: table => new
                {
                    CategoriesCategoryID = table.Column<int>(type: "int", nullable: false),
                    RolesRoleID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles_Permissions", x => new { x.CategoriesCategoryID, x.RolesRoleID });
                    table.ForeignKey(
                        name: "FK_Roles_Permissions_Category_CategoriesCategoryID",
                        column: x => x.CategoriesCategoryID,
                        principalSchema: "BMS",
                        principalTable: "Category",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Roles_Permissions_Role_RolesRoleID",
                        column: x => x.RolesRoleID,
                        principalSchema: "BMS",
                        principalTable: "Role",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "BMS",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserID);
                    table.UniqueConstraint("EmailAddress", x => x.EmailAddress);
                    table.ForeignKey(
                        name: "FK_User_Role_RoleID",
                        column: x => x.RoleID,
                        principalSchema: "BMS",
                        principalTable: "Role",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Chapter",
                schema: "BMS",
                columns: table => new
                {
                    ChapterID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChapterName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chapter", x => x.ChapterID);
                    table.ForeignKey(
                        name: "FK_Chapter_Book_BookID",
                        column: x => x.BookID,
                        principalSchema: "BMS",
                        principalTable: "Book",
                        principalColumn: "BookID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Book_CategoryID",
                schema: "BMS",
                table: "Book",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Chapter_BookID",
                schema: "BMS",
                table: "Chapter",
                column: "BookID");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Permissions_RolesRoleID",
                schema: "BMS",
                table: "Roles_Permissions",
                column: "RolesRoleID");

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleID",
                schema: "BMS",
                table: "User",
                column: "RoleID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chapter",
                schema: "BMS");

            migrationBuilder.DropTable(
                name: "Roles_Permissions",
                schema: "BMS");

            migrationBuilder.DropTable(
                name: "User",
                schema: "BMS");

            migrationBuilder.DropTable(
                name: "Book",
                schema: "BMS");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "BMS");

            migrationBuilder.DropTable(
                name: "Category",
                schema: "BMS");
        }
    }
}
