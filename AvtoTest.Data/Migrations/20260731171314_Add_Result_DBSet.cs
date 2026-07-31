using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AvtoTest.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_Result_DBSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Result_AspNetUsers_CustomUserId",
                table: "Result");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Result",
                table: "Result");

            migrationBuilder.RenameTable(
                name: "Result",
                newName: "Results");

            migrationBuilder.RenameIndex(
                name: "IX_Result_CustomUserId",
                table: "Results",
                newName: "IX_Results_CustomUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Results",
                table: "Results",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Results_AspNetUsers_CustomUserId",
                table: "Results",
                column: "CustomUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Results_AspNetUsers_CustomUserId",
                table: "Results");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Results",
                table: "Results");

            migrationBuilder.RenameTable(
                name: "Results",
                newName: "Result");

            migrationBuilder.RenameIndex(
                name: "IX_Results_CustomUserId",
                table: "Result",
                newName: "IX_Result_CustomUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Result",
                table: "Result",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Result_AspNetUsers_CustomUserId",
                table: "Result",
                column: "CustomUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
