using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Assignment3.Migrations
{
    /// <inheritdoc />
    public partial class FixedImageUrlProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageURL",
                table: "Movie",
                newName: "ImageUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Movie",
                newName: "ImageURL");
        }
    }
}
