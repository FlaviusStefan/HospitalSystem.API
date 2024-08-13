using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalSystem.API.Migrations
{
    /// <inheritdoc />
    public partial class DocSpecHosp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorHospital_Doctors_DoctorId",
                table: "DoctorHospital");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorHospital_Hospitals_HospitalId",
                table: "DoctorHospital");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSpecialization_Doctors_DoctorId",
                table: "DoctorSpecialization");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSpecialization_Specializations_SpecializationId",
                table: "DoctorSpecialization");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DoctorSpecialization",
                table: "DoctorSpecialization");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DoctorHospital",
                table: "DoctorHospital");

            migrationBuilder.RenameTable(
                name: "DoctorSpecialization",
                newName: "DoctorSpecializations");

            migrationBuilder.RenameTable(
                name: "DoctorHospital",
                newName: "DoctorHospitals");

            migrationBuilder.RenameIndex(
                name: "IX_DoctorSpecialization_SpecializationId",
                table: "DoctorSpecializations",
                newName: "IX_DoctorSpecializations_SpecializationId");

            migrationBuilder.RenameIndex(
                name: "IX_DoctorHospital_HospitalId",
                table: "DoctorHospitals",
                newName: "IX_DoctorHospitals_HospitalId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoctorSpecializations",
                table: "DoctorSpecializations",
                columns: new[] { "DoctorId", "SpecializationId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoctorHospitals",
                table: "DoctorHospitals",
                columns: new[] { "DoctorId", "HospitalId" });

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorHospitals_Doctors_DoctorId",
                table: "DoctorHospitals",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorHospitals_Hospitals_HospitalId",
                table: "DoctorHospitals",
                column: "HospitalId",
                principalTable: "Hospitals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSpecializations_Doctors_DoctorId",
                table: "DoctorSpecializations",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSpecializations_Specializations_SpecializationId",
                table: "DoctorSpecializations",
                column: "SpecializationId",
                principalTable: "Specializations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorHospitals_Doctors_DoctorId",
                table: "DoctorHospitals");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorHospitals_Hospitals_HospitalId",
                table: "DoctorHospitals");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSpecializations_Doctors_DoctorId",
                table: "DoctorSpecializations");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSpecializations_Specializations_SpecializationId",
                table: "DoctorSpecializations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DoctorSpecializations",
                table: "DoctorSpecializations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DoctorHospitals",
                table: "DoctorHospitals");

            migrationBuilder.RenameTable(
                name: "DoctorSpecializations",
                newName: "DoctorSpecialization");

            migrationBuilder.RenameTable(
                name: "DoctorHospitals",
                newName: "DoctorHospital");

            migrationBuilder.RenameIndex(
                name: "IX_DoctorSpecializations_SpecializationId",
                table: "DoctorSpecialization",
                newName: "IX_DoctorSpecialization_SpecializationId");

            migrationBuilder.RenameIndex(
                name: "IX_DoctorHospitals_HospitalId",
                table: "DoctorHospital",
                newName: "IX_DoctorHospital_HospitalId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoctorSpecialization",
                table: "DoctorSpecialization",
                columns: new[] { "DoctorId", "SpecializationId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoctorHospital",
                table: "DoctorHospital",
                columns: new[] { "DoctorId", "HospitalId" });

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorHospital_Doctors_DoctorId",
                table: "DoctorHospital",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorHospital_Hospitals_HospitalId",
                table: "DoctorHospital",
                column: "HospitalId",
                principalTable: "Hospitals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSpecialization_Doctors_DoctorId",
                table: "DoctorSpecialization",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSpecialization_Specializations_SpecializationId",
                table: "DoctorSpecialization",
                column: "SpecializationId",
                principalTable: "Specializations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
