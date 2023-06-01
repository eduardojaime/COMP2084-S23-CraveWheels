using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CraveWheels.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRestaurantPK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RestaurantId",
                table: "Restaurants",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Restaurants",
                newName: "RestaurantId");
        }
    }
}
