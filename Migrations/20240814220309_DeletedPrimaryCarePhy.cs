using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalSystem.API.Migrations
{
    /// <inheritdoc />
    public partial class DeletedPrimaryCarePhy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Doctors_PrimaryCarePhysicianId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_PrimaryCarePhysicianId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "PrimaryCarePhysicianId",
                table: "Patients");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Doctors_DoctorId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_DoctorId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "Patients");

            migrationBuilder.AddColumn<Guid>(
                name: "PrimaryCarePhysicianId",
                table: "Patients",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Patients_PrimaryCarePhysicianId",
                table: "Patients",
                column: "PrimaryCarePhysicianId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Doctors_PrimaryCarePhysicianId",
                table: "Patients",
                column: "PrimaryCarePhysicianId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
