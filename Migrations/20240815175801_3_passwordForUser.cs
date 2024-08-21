using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BurgerMania.Migrations
{
    /// <inheritdoc />
    public partial class _3_passwordForUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                schema: "BurgerShopSchemaFF",
                table: "user",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                schema: "BurgerShopSchemaFF",
                table: "user");
        }
    }
}
