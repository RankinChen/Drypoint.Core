using Microsoft.EntityFrameworkCore.Migrations;

namespace Drypoint.EntityFrameworkCore.Migrations
{
    public partial class Update_UserRole_AddRole_User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DrypointUserRoles_RoleId",
                table: "DrypointUserRoles",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_DrypointUserRoles_DrypointRole_RoleId",
                table: "DrypointUserRoles",
                column: "RoleId",
                principalTable: "DrypointRole",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrypointUserRoles_DrypointRole_RoleId",
                table: "DrypointUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_DrypointUserRoles_RoleId",
                table: "DrypointUserRoles");
        }
    }
}
