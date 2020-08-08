using Microsoft.EntityFrameworkCore.Migrations;

namespace Pluto.BlogCore.API.Migrations
{
    public partial class change_post_author : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Post_Title_CategoryId",
                table: "Post");

            migrationBuilder.RenameColumn(
                name: "Author_OpenId",
                table: "Post",
                newName: "AuthorOpenId");

            migrationBuilder.RenameColumn(
                name: "Author_Name",
                table: "Post",
                newName: "AuthorName");

            migrationBuilder.RenameColumn(
                name: "Author_Avatar",
                table: "Post",
                newName: "AuthorAvatar");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorOpenId",
                table: "Post",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AuthorName",
                table: "Post",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AuthorAvatar",
                table: "Post",
                maxLength: 512,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Post_Id_Title_CategoryId",
                table: "Post",
                columns: new[] { "Id", "Title", "CategoryId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Post_Id_Title_CategoryId",
                table: "Post");

            migrationBuilder.RenameColumn(
                name: "AuthorOpenId",
                table: "Post",
                newName: "Author_OpenId");

            migrationBuilder.RenameColumn(
                name: "AuthorName",
                table: "Post",
                newName: "Author_Name");

            migrationBuilder.RenameColumn(
                name: "AuthorAvatar",
                table: "Post",
                newName: "Author_Avatar");

            migrationBuilder.AlterColumn<string>(
                name: "Author_OpenId",
                table: "Post",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Author_Name",
                table: "Post",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Author_Avatar",
                table: "Post",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 512,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Post_Title_CategoryId",
                table: "Post",
                columns: new[] { "Title", "CategoryId" });
        }
    }
}
