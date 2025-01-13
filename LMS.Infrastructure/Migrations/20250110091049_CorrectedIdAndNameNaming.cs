using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CorrectedIdAndNameNaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ActivityTypes",
                newName: "ActivityTypeName");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ActivityTypes",
                newName: "ActivityTypeId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Activities",
                newName: "ActivityName");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Activities",
                newName: "ActivityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ActivityTypeName",
                table: "ActivityTypes",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "ActivityTypeId",
                table: "ActivityTypes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ActivityName",
                table: "Activities",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "ActivityId",
                table: "Activities",
                newName: "Id");
        }
    }
}
