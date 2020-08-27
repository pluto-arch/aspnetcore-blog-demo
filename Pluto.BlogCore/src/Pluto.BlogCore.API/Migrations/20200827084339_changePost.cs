using Microsoft.EntityFrameworkCore.Migrations;

namespace Pluto.BlogCore.API.Migrations
{
    public partial class changePost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorAvatar",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "AuthorName",
                table: "Post");

            migrationBuilder.RenameColumn(
                name: "AuthorOpenId",
                table: "Post",
                newName: "OpenId");

            migrationBuilder.AddColumn<string>(
                name: "Avator",
                table: "YuqueAuth",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThirdOpenid",
                table: "Post",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Format",
                table: "Post",
                type: "nvarchar(32)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Platform",
                table: "Post",
                type: "nvarchar(32)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlatformId",
                table: "Post",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HtmlContent",
                table: "Post",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "Post",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MarkdownContent",
                table: "Post",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostType",
                table: "Post",
                type: "nvarchar(16)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avator",
                table: "YuqueAuth");

            migrationBuilder.DropColumn(
                name: "ThirdOpenid",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "Format",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "Platform",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "PlatformId",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "HtmlContent",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "Link",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "MarkdownContent",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "PostType",
                table: "Post");

            migrationBuilder.RenameColumn(
                name: "OpenId",
                table: "Post",
                newName: "AuthorOpenId");

            migrationBuilder.AddColumn<string>(
                name: "AuthorAvatar",
                table: "Post",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuthorName",
                table: "Post",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);
        }
    }
}
