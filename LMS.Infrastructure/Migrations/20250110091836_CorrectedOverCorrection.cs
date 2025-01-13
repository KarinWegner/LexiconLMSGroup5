using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CorrectedOverCorrection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ActivityTypeName",
                table: "ActivityTypes",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "ActivityName",
                table: "Activities",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ActivityTypes",
                newName: "ActivityTypeName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Activities",
                newName: "ActivityName");
        }
    }
}
