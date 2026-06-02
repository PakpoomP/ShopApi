using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterviewTTBApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateStatusCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "CartItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "CartItems");
        }
    }
}
