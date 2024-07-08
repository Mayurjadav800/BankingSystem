using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentMicroServices.Migrations
{
    /// <inheritdoc />
    public partial class AddedCompundmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Compund",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Principal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Rate = table.Column<double>(type: "float", nullable: false),
                    Time = table.Column<int>(type: "int", nullable: false),
                    Interest = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compund", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Compund");
        }
    }
}
