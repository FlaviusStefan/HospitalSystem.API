using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalSystem.API.Migrations
{
    /// <inheritdoc />
    public partial class QualificationYOCdateTOint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add a new temporary column to store the integer values
            migrationBuilder.AddColumn<int>(
                name: "YearOfCompletionInt",
                table: "Qualifications",
                nullable: false,
                defaultValue: 0);

            // Copy the year from the existing datetime column to the new int column
            migrationBuilder.Sql(
                "UPDATE Qualifications SET YearOfCompletionInt = YEAR(YearOfCompletion)");

            // Drop the old datetime2 column
            migrationBuilder.DropColumn(
                name: "YearOfCompletion",
                table: "Qualifications");

            // Rename the new int column to the original column name
            migrationBuilder.RenameColumn(
                name: "YearOfCompletionInt",
                table: "Qualifications",
                newName: "YearOfCompletion");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Add the old datetime2 column back
            migrationBuilder.AddColumn<DateTime>(
                name: "YearOfCompletion",
                table: "Qualifications",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            // Copy the integer year values back to datetime2
            migrationBuilder.Sql(
                "UPDATE Qualifications SET YearOfCompletion = DATEFROMPARTS(YearOfCompletion, 1, 1)");

            // Drop the new int column
            migrationBuilder.DropColumn(
                name: "YearOfCompletion",
                table: "Qualifications");

            // Rename the datetime2 column back to the original name
            migrationBuilder.RenameColumn(
                name: "YearOfCompletionInt",
                table: "Qualifications",
                newName: "YearOfCompletion");
        }
    }
}
