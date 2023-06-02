using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookManagementSystem_BMS.Migrations
{
    public partial class updateTableRolePermissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Permissions_Category_CategoriesCategoryID",
                schema: "BMS",
                table: "Roles_Permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Permissions_Role_RolesRoleID",
                schema: "BMS",
                table: "Roles_Permissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles_Permissions",
                schema: "BMS",
                table: "Roles_Permissions");

            migrationBuilder.RenameTable(
                name: "Roles_Permissions",
                schema: "BMS",
                newName: "Roles_Categories",
                newSchema: "BMS");

            migrationBuilder.RenameIndex(
                name: "IX_Roles_Permissions_RolesRoleID",
                schema: "BMS",
                table: "Roles_Categories",
                newName: "IX_Roles_Categories_RolesRoleID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles_Categories",
                schema: "BMS",
                table: "Roles_Categories",
                columns: new[] { "CategoriesCategoryID", "RolesRoleID" });

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Categories_Category_CategoriesCategoryID",
                schema: "BMS",
                table: "Roles_Categories",
                column: "CategoriesCategoryID",
                principalSchema: "BMS",
                principalTable: "Category",
                principalColumn: "CategoryID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Categories_Role_RolesRoleID",
                schema: "BMS",
                table: "Roles_Categories",
                column: "RolesRoleID",
                principalSchema: "BMS",
                principalTable: "Role",
                principalColumn: "RoleID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Categories_Category_CategoriesCategoryID",
                schema: "BMS",
                table: "Roles_Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Categories_Role_RolesRoleID",
                schema: "BMS",
                table: "Roles_Categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles_Categories",
                schema: "BMS",
                table: "Roles_Categories");

            migrationBuilder.RenameTable(
                name: "Roles_Categories",
                schema: "BMS",
                newName: "Roles_Permissions",
                newSchema: "BMS");

            migrationBuilder.RenameIndex(
                name: "IX_Roles_Categories_RolesRoleID",
                schema: "BMS",
                table: "Roles_Permissions",
                newName: "IX_Roles_Permissions_RolesRoleID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles_Permissions",
                schema: "BMS",
                table: "Roles_Permissions",
                columns: new[] { "CategoriesCategoryID", "RolesRoleID" });

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Permissions_Category_CategoriesCategoryID",
                schema: "BMS",
                table: "Roles_Permissions",
                column: "CategoriesCategoryID",
                principalSchema: "BMS",
                principalTable: "Category",
                principalColumn: "CategoryID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Permissions_Role_RolesRoleID",
                schema: "BMS",
                table: "Roles_Permissions",
                column: "RolesRoleID",
                principalSchema: "BMS",
                principalTable: "Role",
                principalColumn: "RoleID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
