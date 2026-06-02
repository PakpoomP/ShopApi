using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterviewTTBApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changeCartIdToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE \"CartItems\" ALTER COLUMN \"CartId\" TYPE text USING \"CartId\"::text;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE \"CartItems\" ALTER COLUMN \"CartId\" TYPE uuid USING \"CartId\"::uuid;");
        }
    }
}
