using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryProject.Data.Migrations
{
    public partial class modelsupdated2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookRecord_AspNetUsers_LibraryUserId1",
                table: "BookRecord");

            migrationBuilder.DropIndex(
                name: "IX_BookRecord_LibraryUserId1",
                table: "BookRecord");

            migrationBuilder.DropColumn(
                name: "LibraryUserId1",
                table: "BookRecord");

            migrationBuilder.AlterColumn<string>(
                name: "LibraryUserId",
                table: "BookRecord",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_BookRecord_LibraryUserId",
                table: "BookRecord",
                column: "LibraryUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookRecord_AspNetUsers_LibraryUserId",
                table: "BookRecord",
                column: "LibraryUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookRecord_AspNetUsers_LibraryUserId",
                table: "BookRecord");

            migrationBuilder.DropIndex(
                name: "IX_BookRecord_LibraryUserId",
                table: "BookRecord");

            migrationBuilder.AlterColumn<int>(
                name: "LibraryUserId",
                table: "BookRecord",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LibraryUserId1",
                table: "BookRecord",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookRecord_LibraryUserId1",
                table: "BookRecord",
                column: "LibraryUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_BookRecord_AspNetUsers_LibraryUserId1",
                table: "BookRecord",
                column: "LibraryUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
