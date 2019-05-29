using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryProject.Data.Migrations
{
    public partial class modelsupdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookRecord_Books_BookId",
                table: "BookRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_BookRecord_AspNetUsers_UserId",
                table: "BookRecord");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "Books",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "BookRecord",
                newName: "LibraryUserId1");

            migrationBuilder.RenameColumn(
                name: "BookRecordId",
                table: "BookRecord",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_BookRecord_UserId",
                table: "BookRecord",
                newName: "IX_BookRecord_LibraryUserId1");

            migrationBuilder.AlterColumn<int>(
                name: "BookId",
                table: "BookRecord",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LibraryUserId",
                table: "BookRecord",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_BookRecord_Books_BookId",
                table: "BookRecord",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookRecord_AspNetUsers_LibraryUserId1",
                table: "BookRecord",
                column: "LibraryUserId1",
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
                name: "FK_BookRecord_AspNetUsers_LibraryUserId1",
                table: "BookRecord");

            migrationBuilder.DropColumn(
                name: "LibraryUserId",
                table: "BookRecord");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Books",
                newName: "BookId");

            migrationBuilder.RenameColumn(
                name: "LibraryUserId1",
                table: "BookRecord",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "BookRecord",
                newName: "BookRecordId");

            migrationBuilder.RenameIndex(
                name: "IX_BookRecord_LibraryUserId1",
                table: "BookRecord",
                newName: "IX_BookRecord_UserId");

            migrationBuilder.AlterColumn<int>(
                name: "BookId",
                table: "BookRecord",
                nullable: true,
                oldClrType: typeof(int));

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
    }
}
