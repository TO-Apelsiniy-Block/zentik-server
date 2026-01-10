using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZenticServer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "text",
                table: "message");

            migrationBuilder.AddColumn<string>(
                name: "type",
                table: "message",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "message_text",
                columns: table => new
                {
                    MessageId = table.Column<int>(type: "integer", nullable: false),
                    text = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_message_text", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_message_text_message_MessageId",
                        column: x => x.MessageId,
                        principalTable: "message",
                        principalColumn: "message_id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "message_text");

            migrationBuilder.DropColumn(
                name: "type",
                table: "message");

            migrationBuilder.AddColumn<string>(
                name: "text",
                table: "message",
                type: "character varying(4096)",
                maxLength: 4096,
                nullable: false,
                defaultValue: "");
        }
    }
}
