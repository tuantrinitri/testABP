using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.IdentityService.Migrations
{
    /// <inheritdoc />
    public partial class AddUserProfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserOrganizationUnits_UserId_OrganizationUnitId",
                table: "UserOrganizationUnits");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationUnitRoles_RoleId_OrganizationUnitId",
                table: "OrganizationUnitRoles");

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedUserName",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" });

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    SoldierCode = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    DateOfBirth = table.Column<long>(type: "bigint", nullable: false),
                    StartDateMilitary = table.Column<long>(type: "bigint", nullable: false),
                    EndDateMilitary = table.Column<long>(type: "bigint", nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOrganizationUnits_OrganizationUnitId_UserId",
                table: "UserOrganizationUnits",
                columns: new[] { "OrganizationUnitId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserOrganizationUnits_UserId",
                table: "UserOrganizationUnits",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationUnitRoles_OrganizationUnitId_RoleId",
                table: "OrganizationUnitRoles",
                columns: new[] { "OrganizationUnitId", "RoleId" });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationUnitRoles_RoleId",
                table: "OrganizationUnitRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_UserId",
                table: "UserProfiles",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserOrganizationUnits_OrganizationUnitId_UserId",
                table: "UserOrganizationUnits");

            migrationBuilder.DropIndex(
                name: "IX_UserOrganizationUnits_UserId",
                table: "UserOrganizationUnits");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationUnitRoles_OrganizationUnitId_RoleId",
                table: "OrganizationUnitRoles");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationUnitRoles_RoleId",
                table: "OrganizationUnitRoles");

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedUserName",
                table: "Users",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserOrganizationUnits_UserId_OrganizationUnitId",
                table: "UserOrganizationUnits",
                columns: new[] { "UserId", "OrganizationUnitId" });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationUnitRoles_RoleId_OrganizationUnitId",
                table: "OrganizationUnitRoles",
                columns: new[] { "RoleId", "OrganizationUnitId" });
        }
    }
}
