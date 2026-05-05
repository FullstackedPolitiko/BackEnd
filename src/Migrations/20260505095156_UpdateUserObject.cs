using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserObject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Token",
                table: "Users",
                newName: "GoogleID");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Token",
                table: "Users",
                newName: "IX_Users_GoogleID");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "GoogleID",
                table: "Users",
                newName: "Token");

            migrationBuilder.RenameIndex(
                name: "IX_Users_GoogleID",
                table: "Users",
                newName: "IX_Users_Token");
        }
    }
}
