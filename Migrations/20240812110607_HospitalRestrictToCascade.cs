using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalSystem.API.Migrations
{
    /// <inheritdoc />
    public partial class HospitalRestrictToCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hospitals_Addresses_AddressId",
                table: "Hospitals");

            migrationBuilder.DropForeignKey(
                name: "FK_Hospitals_Contacts_ContactId",
                table: "Hospitals");

            migrationBuilder.AddForeignKey(
                name: "FK_Hospitals_Addresses_AddressId",
                table: "Hospitals",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Hospitals_Contacts_ContactId",
                table: "Hospitals",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hospitals_Addresses_AddressId",
                table: "Hospitals");

            migrationBuilder.DropForeignKey(
                name: "FK_Hospitals_Contacts_ContactId",
                table: "Hospitals");

            migrationBuilder.AddForeignKey(
                name: "FK_Hospitals_Addresses_AddressId",
                table: "Hospitals",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Hospitals_Contacts_ContactId",
                table: "Hospitals",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
