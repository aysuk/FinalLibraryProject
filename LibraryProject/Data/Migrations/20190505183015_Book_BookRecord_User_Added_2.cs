using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryProject.Data.Migrations
{
    public partial class Book_BookRecord_User_Added_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookRecord_Books_BookId",
                table: "BookRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_BookRecord_AspNetUsers_UserId1",
                table: "BookRecord");

            migrationBuilder.DropIndex(
                name: "IX_BookRecord_UserId1",
                table: "BookRecord");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "BookRecord");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "BookRecord",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "BookId",
                table: "BookRecord",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_BookRecord_UserId",
                table: "BookRecord",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookRecord_Books_BookId",
                table: "BookRecord",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BookRecord_AspNetUsers_UserId",
                table: "BookRecord",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookRecord_Books_BookId",
                table: "BookRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_BookRecord_AspNetUsers_UserId",
                table: "BookRecord");

            migrationBuilder.DropIndex(
                name: "IX_BookRecord_UserId",
                table: "BookRecord");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "BookRecord",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BookId",
                table: "BookRecord",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "BookRecord",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookRecord_UserId1",
                table: "BookRecord",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_BookRecord_Books_BookId",
                table: "BookRecord",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookRecord_AspNetUsers_UserId1",
                table: "BookRecord",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
