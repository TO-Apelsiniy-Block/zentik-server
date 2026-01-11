using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZenticServer.Migrations
{
    /// <inheritdoc />
    public partial class FixAbsenceFielfRoleToChatUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "role",
                table: "chat_user",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "role",
                table: "chat_user");
        }
    }
}
