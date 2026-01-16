using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VianaHub.Global.Gerit.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixCreatedByModifiedByToInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Roles_RoleId1",
                schema: "dbo",
                table: "UserRoles");

            migrationBuilder.RenameColumn(
                name: "RoleId1",
                schema: "dbo",
                table: "UserRoles",
                newName: "RoleEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_RoleId1",
                schema: "dbo",
                table: "UserRoles",
                newName: "IX_UserRoles_RoleEntityId");

            migrationBuilder.AlterColumn<int>(
                name: "ModifiedBy",
                schema: "dbo",
                table: "UserRoles",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                schema: "dbo",
                table: "UserRoles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModifiedBy",
                schema: "dbo",
                table: "RolePermissions",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                schema: "dbo",
                table: "RolePermissions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModifiedBy",
                schema: "dbo",
                table: "Actions",
                type: "INT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                schema: "dbo",
                table: "Actions",
                type: "INT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "dbo",
                table: "Actions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "JwtKeys",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    KeyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PublicKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrivateKeyEncrypted = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Algorithm = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    KeySize = table.Column<int>(type: "int", nullable: false),
                    KeyType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RevokedReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UsageCount = table.Column<long>(type: "bigint", nullable: false),
                    ActivatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUsedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NextRotationAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastValidatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ValidationCount = table.Column<long>(type: "bigint", nullable: false),
                    RotationPolicyDays = table.Column<int>(type: "int", nullable: false),
                    OverlapPeriodDays = table.Column<int>(type: "int", nullable: false),
                    MaxTokenLifetimeMinutes = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JwtKeys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JwtKeys_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JwtKeys_Expiration",
                schema: "dbo",
                table: "JwtKeys",
                column: "ExpiresAt",
                filter: "IsActive = 1 AND IsDeleted = 0 AND RevokedAt IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_JwtKeys_IsActive_IsDeleted",
                schema: "dbo",
                table: "JwtKeys",
                columns: new[] { "IsActive", "IsDeleted" },
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_JwtKeys_KeyId_Lookup",
                schema: "dbo",
                table: "JwtKeys",
                column: "KeyId",
                unique: true,
                filter: "IsDeleted = 0 AND RevokedAt IS NULL")
                .Annotation("SqlServer:Include", new[] { "Algorithm", "PublicKey" });

            migrationBuilder.CreateIndex(
                name: "IX_JwtKeys_NextRotation",
                schema: "dbo",
                table: "JwtKeys",
                column: "NextRotationAt",
                filter: "IsActive = 1 AND IsDeleted = 0 AND RevokedAt IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_JwtKeys_TenantId",
                schema: "dbo",
                table: "JwtKeys",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Roles_RoleEntityId",
                schema: "dbo",
                table: "UserRoles",
                column: "RoleEntityId",
                principalSchema: "dbo",
                principalTable: "Roles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Roles_RoleEntityId",
                schema: "dbo",
                table: "UserRoles");

            migrationBuilder.DropTable(
                name: "JwtKeys",
                schema: "dbo");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "dbo",
                table: "Actions");

            migrationBuilder.RenameColumn(
                name: "RoleEntityId",
                schema: "dbo",
                table: "UserRoles",
                newName: "RoleId1");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_RoleEntityId",
                schema: "dbo",
                table: "UserRoles",
                newName: "IX_UserRoles_RoleId1");

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                schema: "dbo",
                table: "UserRoles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                schema: "dbo",
                table: "UserRoles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                schema: "dbo",
                table: "RolePermissions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                schema: "dbo",
                table: "RolePermissions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                schema: "dbo",
                table: "Actions",
                type: "NVARCHAR(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                schema: "dbo",
                table: "Actions",
                type: "NVARCHAR(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INT");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Roles_RoleId1",
                schema: "dbo",
                table: "UserRoles",
                column: "RoleId1",
                principalSchema: "dbo",
                principalTable: "Roles",
                principalColumn: "Id");
        }
    }
}
