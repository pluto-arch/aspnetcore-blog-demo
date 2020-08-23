using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pluto.BlogCore.API.Migrations
{
    public partial class yuqueTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThirsAuthorizeInfo");

            migrationBuilder.CreateTable(
                name: "YuqueAuth",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()"),
                    ModifyTime = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()"),
                    OpenId = table.Column<string>(maxLength: 300, nullable: false),
                    AccessToken = table.Column<string>(maxLength: 1024, nullable: false),
                    PlatformOpenId = table.Column<string>(maxLength: 1024, nullable: false),
                    PlatformName = table.Column<string>(maxLength: 300, nullable: true),
                    RefreshToken = table.Column<string>(maxLength: 1024, nullable: false),
                    Expired = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YuqueAuth", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_YuqueAuth_OpenId_PlatformOpenId",
                table: "YuqueAuth",
                columns: new[] { "OpenId", "PlatformOpenId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "YuqueAuth");

            migrationBuilder.CreateTable(
                name: "ThirsAuthorizeInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccessToken = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Expired = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    OpenId = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    PlatformName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    PlatformOpenId = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    PlatformType = table.Column<string>(type: "nvarchar(32)", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThirsAuthorizeInfo", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ThirsAuthorizeInfo_OpenId_PlatformOpenId",
                table: "ThirsAuthorizeInfo",
                columns: new[] { "OpenId", "PlatformOpenId" });
        }
    }
}
