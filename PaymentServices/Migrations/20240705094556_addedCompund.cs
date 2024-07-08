using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentMicroServices.Migrations
{
    /// <inheritdoc />
    public partial class addedCompund : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompoundingsPerYear",
                table: "Compund",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompoundingsPerYear",
                table: "Compund");
        }
    }
}
