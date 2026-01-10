using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZenticServer.Migrations
{
    /// <inheritdoc />
    public partial class AddUpdatedAtTriger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
@"CREATE OR REPLACE FUNCTION set_updated_at()
RETURNS trigger
LANGUAGE plpgsql
AS $$
BEGIN
    NEW.updated_at := CURRENT_TIMESTAMP;
    RETURN NEW;
END;
$$;"
            );
            
            migrationBuilder.Sql(
                @"CREATE TRIGGER user_set_updated_at
BEFORE UPDATE ON ""user""
FOR EACH ROW
EXECUTE FUNCTION set_updated_at();"
            );
            migrationBuilder.Sql(
                @"CREATE TRIGGER chat_set_updated_at
BEFORE UPDATE ON ""chat""
FOR EACH ROW
EXECUTE FUNCTION set_updated_at();"
            );
            migrationBuilder.Sql(
                @"CREATE TRIGGER chat_user_set_updated_at
BEFORE UPDATE ON ""chat_user""
FOR EACH ROW
EXECUTE FUNCTION set_updated_at();"
            );
            migrationBuilder.Sql(
                @"CREATE TRIGGER message_set_updated_at
BEFORE UPDATE ON ""message""
FOR EACH ROW
EXECUTE FUNCTION set_updated_at();"
            );
            migrationBuilder.Sql(
                @"CREATE TRIGGER email_confirmation_set_updated_at
BEFORE UPDATE ON ""email_confirmation""
FOR EACH ROW
EXECUTE FUNCTION set_updated_at();"
            );
            
        }
        

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
