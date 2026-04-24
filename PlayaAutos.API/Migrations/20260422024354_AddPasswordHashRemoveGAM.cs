using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlayaAutos.API.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordHashRemoveGAM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GAMUserId",
                table: "Usuarios");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Usuarios");

            migrationBuilder.AddColumn<string>(
                name: "GAMUserId",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
