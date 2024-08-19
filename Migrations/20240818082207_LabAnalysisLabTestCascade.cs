using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalSystem.API.Migrations
{
    /// <inheritdoc />
    public partial class LabAnalysisLabTestCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabAnalyses_Patients_PatientId",
                table: "LabAnalyses");

            migrationBuilder.DropForeignKey(
                name: "FK_LabTests_LabAnalyses_LabAnalysisId",
                table: "LabTests");

            migrationBuilder.AddForeignKey(
                name: "FK_LabAnalyses_Patients_PatientId",
                table: "LabAnalyses",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LabTests_LabAnalyses_LabAnalysisId",
                table: "LabTests",
                column: "LabAnalysisId",
                principalTable: "LabAnalyses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabAnalyses_Patients_PatientId",
                table: "LabAnalyses");

            migrationBuilder.DropForeignKey(
                name: "FK_LabTests_LabAnalyses_LabAnalysisId",
                table: "LabTests");

            migrationBuilder.AddForeignKey(
                name: "FK_LabAnalyses_Patients_PatientId",
                table: "LabAnalyses",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LabTests_LabAnalyses_LabAnalysisId",
                table: "LabTests",
                column: "LabAnalysisId",
                principalTable: "LabAnalyses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
