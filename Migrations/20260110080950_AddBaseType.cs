using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZenticServer.Migrations
{
    /// <inheritdoc />
    public partial class AddBaseType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "user",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "user",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "message",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "message",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "email_confirmation",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "email_confirmation",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "chat_user",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "chat_user",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "chat",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "chat",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_at",
                table: "user");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "user");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "message");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "message");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "email_confirmation");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "email_confirmation");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "chat_user");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "chat_user");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "chat");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "chat");
        }
    }
}
