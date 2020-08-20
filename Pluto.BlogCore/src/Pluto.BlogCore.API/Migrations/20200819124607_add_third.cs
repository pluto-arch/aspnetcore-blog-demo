using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pluto.BlogCore.API.Migrations
{
    public partial class add_third : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Post_Id_Title_CategoryId",
                table: "Post");

            migrationBuilder.CreateTable(
                name: "ThirsAuthorizeInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: false),
                    PlatformType = table.Column<string>(type: "nvarchar(32)", nullable: false),
                    OpenId = table.Column<string>(maxLength: 300, nullable: false),
                    AccessToken = table.Column<string>(maxLength: 1024, nullable: false),
                    RefreshToken = table.Column<string>(maxLength: 1024, nullable: false),
                    Expired = table.Column<DateTime>(nullable: true),
                    PlatformOpenId = table.Column<string>(maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThirsAuthorizeInfo", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Post_Id_Title",
                table: "Post",
                columns: new[] { "Id", "Title" });

            migrationBuilder.CreateIndex(
                name: "IX_ThirsAuthorizeInfo_OpenId_PlatformOpenId",
                table: "ThirsAuthorizeInfo",
                columns: new[] { "OpenId", "PlatformOpenId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThirsAuthorizeInfo");

            migrationBuilder.DropIndex(
                name: "IX_Post_Id_Title",
                table: "Post");

            migrationBuilder.CreateIndex(
                name: "IX_Post_Id_Title_CategoryId",
                table: "Post",
                columns: new[] { "Id", "Title", "CategoryId" });
        }
    }
}
