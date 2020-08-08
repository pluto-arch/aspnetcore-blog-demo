using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pluto.BlogCore.API.Migrations
{
    public partial class change_init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PostTag",
                table: "PostTag");

            migrationBuilder.DropIndex(
                name: "IX_PostTag_PostId",
                table: "PostTag");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PostTag");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateTime",
                table: "Tag",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifyTime",
                table: "Tag",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateTime",
                table: "Post",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifyTime",
                table: "Post",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateTime",
                table: "Category",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifyTime",
                table: "Category",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostTag",
                table: "PostTag",
                columns: new[] { "PostId", "TagId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PostTag",
                table: "PostTag");

            migrationBuilder.DropColumn(
                name: "CreateTime",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "ModifyTime",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "CreateTime",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "ModifyTime",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "CreateTime",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "ModifyTime",
                table: "Category");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "PostTag",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostTag",
                table: "PostTag",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PostTag_PostId",
                table: "PostTag",
                column: "PostId");
        }
    }
}
