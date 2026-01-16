using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VianaHub.Global.Gerit.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Actions",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    ModifiedBy = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Resources",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    ModifiedBy = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LegalName = table.Column<string>(type: "NVARCHAR(200)", maxLength: 200, nullable: false),
                    TradeName = table.Column<string>(type: "NVARCHAR(200)", maxLength: 200, nullable: true),
                    Consent = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    IsActive = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    ModifiedBy = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR(255)", maxLength: 255, nullable: true),
                    Phone = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false),
                    Consent = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    IsActive = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    ModifiedBy = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clients_Tenant",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Equipments",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR(150)", maxLength: 150, nullable: false),
                    SerialNumber = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    ModifiedBy = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Equipments_Tenant",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    ModifiedBy = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Roles_Tenant",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeamMembers",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR(150)", maxLength: 150, nullable: false),
                    TaxNumber = table.Column<string>(type: "NVARCHAR(20)", maxLength: 20, nullable: true),
                    IsActive = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    ModifiedBy = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamMembers_Tenant",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TenantAddresses",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    Street = table.Column<string>(type: "NVARCHAR(200)", maxLength: 200, nullable: false),
                    City = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    PostalCode = table.Column<string>(type: "NVARCHAR(20)", maxLength: 20, nullable: false),
                    District = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: true),
                    CountryCode = table.Column<string>(type: "CHAR(2)", maxLength: 2, nullable: false, defaultValue: "PT"),
                    IsPrimary = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    ModifiedBy = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenantAddresses_Tenant",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TenantContacts",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "NVARCHAR(30)", maxLength: 30, nullable: true),
                    IsPrimary = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    ModifiedBy = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenantContacts_Tenant",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TenantFiscalData",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    NIF = table.Column<string>(type: "CHAR(9)", maxLength: 9, nullable: false),
                    VATNumber = table.Column<string>(type: "NVARCHAR(20)", maxLength: 20, nullable: false),
                    CAE = table.Column<string>(type: "NVARCHAR(10)", maxLength: 10, nullable: true),
                    FiscalCountry = table.Column<string>(type: "CHAR(2)", maxLength: 2, nullable: false, defaultValue: "PT"),
                    IsVATRegistered = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    IsActive = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    ModifiedBy = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantFiscalData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenantFiscalData_Tenant",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "VARBINARY(64)", maxLength: 64, nullable: false),
                    FullName = table.Column<string>(type: "NVARCHAR(150)", maxLength: 150, nullable: false),
                    IsActive = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    ModifiedBy = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Tenant",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    Plate = table.Column<string>(type: "NVARCHAR(20)", maxLength: 20, nullable: false),
                    Model = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    ModifiedBy = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_Tenants",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClientAddresses",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    Street = table.Column<string>(type: "NVARCHAR(200)", maxLength: 200, nullable: false),
                    City = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    PostalCode = table.Column<string>(type: "NVARCHAR(20)", maxLength: 20, nullable: false),
                    District = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: true),
                    CountryCode = table.Column<string>(type: "CHAR(2)", maxLength: 2, nullable: false, defaultValue: "PT"),
                    IsPrimary = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    ModifiedBy = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientAddresses_Client",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientAddresses_Tenant",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClientContacts",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "NVARCHAR(30)", maxLength: 30, nullable: true),
                    IsPrimary = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    ModifiedBy = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientContacts_Client",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientContacts_Tenant",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClientFiscalData",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    NIF = table.Column<string>(type: "CHAR(9)", maxLength: 9, nullable: false),
                    VATNumber = table.Column<string>(type: "NVARCHAR(20)", maxLength: 20, nullable: false),
                    CAE = table.Column<string>(type: "NVARCHAR(10)", maxLength: 10, nullable: true),
                    FiscalCountry = table.Column<string>(type: "CHAR(2)", maxLength: 2, nullable: false, defaultValue: "PT"),
                    IsVATRegistered = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    IsActive = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    ModifiedBy = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientFiscalData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientFiscalData_Client",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientFiscalData_Tenant",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ResourceId = table.Column<int>(type: "int", nullable: false),
                    ActionId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Action",
                        column: x => x.ActionId,
                        principalSchema: "dbo",
                        principalTable: "Actions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Resource",
                        column: x => x.ResourceId,
                        principalSchema: "dbo",
                        principalTable: "Resources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Role",
                        column: x => x.RoleId,
                        principalSchema: "dbo",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Tenant",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeamMemberAddresses",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    TeamMemberId = table.Column<int>(type: "int", nullable: false),
                    Street = table.Column<string>(type: "NVARCHAR(200)", maxLength: 200, nullable: false),
                    City = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    PostalCode = table.Column<string>(type: "NVARCHAR(20)", maxLength: 20, nullable: false),
                    District = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: true),
                    CountryCode = table.Column<string>(type: "CHAR(2)", maxLength: 2, nullable: false, defaultValue: "PT"),
                    IsPrimary = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    ModifiedBy = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMemberAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamMemberAddresses_TeamMember",
                        column: x => x.TeamMemberId,
                        principalSchema: "dbo",
                        principalTable: "TeamMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeamMemberAddresses_Tenant",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeamMemberContacts",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    TeamMemberId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "NVARCHAR(30)", maxLength: 30, nullable: true),
                    IsPrimary = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    ModifiedBy = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMemberContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamMemberContacts_TeamMember",
                        column: x => x.TeamMemberId,
                        principalSchema: "dbo",
                        principalTable: "TeamMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeamMemberContacts_Tenant",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    RoleId1 = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_Role",
                        column: x => x.RoleId,
                        principalSchema: "dbo",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId1",
                        column: x => x.RoleId1,
                        principalSchema: "dbo",
                        principalTable: "Roles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserRoles_Tenant",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoles_User",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Interventions",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    TeamMemberId = table.Column<int>(type: "int", nullable: false),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "NVARCHAR(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR(2000)", maxLength: 2000, nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    EstimatedValue = table.Column<decimal>(type: "DECIMAL(10,2)", precision: 10, scale: 2, nullable: false),
                    RealValue = table.Column<decimal>(type: "DECIMAL(10,2)", precision: 10, scale: 2, nullable: true),
                    Status = table.Column<byte>(type: "TINYINT", nullable: false),
                    IsActive = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    ModifiedBy = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interventions", x => x.Id);
                    table.CheckConstraint("CK_EstimatedValue", "[EstimatedValue] >= 0");
                    table.CheckConstraint("CK_Interventions_EndDateTime", "[EndDateTime] IS NULL OR [EndDateTime] >= [StartDateTime]");
                    table.ForeignKey(
                        name: "FK_Interventions_Clients",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Interventions_TeamMembers",
                        column: x => x.TeamMemberId,
                        principalSchema: "dbo",
                        principalTable: "TeamMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Interventions_Tenants",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Interventions_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalSchema: "dbo",
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InterventionAddresses",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    InterventionId = table.Column<int>(type: "int", nullable: false),
                    Street = table.Column<string>(type: "NVARCHAR(200)", maxLength: 200, nullable: false),
                    City = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    PostalCode = table.Column<string>(type: "NVARCHAR(20)", maxLength: 20, nullable: false),
                    District = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: true),
                    CountryCode = table.Column<string>(type: "CHAR(2)", maxLength: 2, nullable: false, defaultValue: "PT"),
                    IsPrimary = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    ModifiedBy = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterventionAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterventionAddresses_Intervention",
                        column: x => x.InterventionId,
                        principalSchema: "dbo",
                        principalTable: "Interventions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InterventionAddresses_Tenant",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InterventionContacts",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    InterventionId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "NVARCHAR(30)", maxLength: 30, nullable: true),
                    IsPrimary = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    ModifiedBy = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterventionContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterventionContacts_Intervention",
                        column: x => x.InterventionId,
                        principalSchema: "dbo",
                        principalTable: "Interventions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InterventionContacts_Tenant",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "UQ_Actions_Name",
                schema: "dbo",
                table: "Actions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientAddresses_ClientId",
                schema: "dbo",
                table: "ClientAddresses",
                column: "ClientId",
                unique: true,
                filter: "[IsDeleted] = 0")
                .Annotation("SqlServer:Include", new[] { "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ClientAddresses_TenantId",
                schema: "dbo",
                table: "ClientAddresses",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientContacts_ClientId",
                schema: "dbo",
                table: "ClientContacts",
                column: "ClientId",
                filter: "[IsDeleted] = 0")
                .Annotation("SqlServer:Include", new[] { "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ClientContacts_TenantId",
                schema: "dbo",
                table: "ClientContacts",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientFiscalData_ClientId",
                schema: "dbo",
                table: "ClientFiscalData",
                column: "ClientId",
                filter: "[IsDeleted] = 0")
                .Annotation("SqlServer:Include", new[] { "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ClientFiscalData_TenantId",
                schema: "dbo",
                table: "ClientFiscalData",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "UQ_ClientFiscalData_NIF",
                schema: "dbo",
                table: "ClientFiscalData",
                column: "NIF",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Tenant_Active",
                schema: "dbo",
                table: "Clients",
                columns: new[] { "TenantId", "Name" },
                unique: true,
                filter: "[IsDeleted] = 0")
                .Annotation("SqlServer:Include", new[] { "Email", "Phone" });

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_TenantId",
                schema: "dbo",
                table: "Equipments",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_InterventionAddresses_InterventionId",
                schema: "dbo",
                table: "InterventionAddresses",
                column: "InterventionId",
                filter: "[IsDeleted] = 0")
                .Annotation("SqlServer:Include", new[] { "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_InterventionAddresses_TenantId",
                schema: "dbo",
                table: "InterventionAddresses",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_InterventionContacts_InterventionId",
                schema: "dbo",
                table: "InterventionContacts",
                column: "InterventionId",
                filter: "[IsDeleted] = 0")
                .Annotation("SqlServer:Include", new[] { "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_InterventionContacts_TenantId",
                schema: "dbo",
                table: "InterventionContacts",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Interventions_ClientId",
                schema: "dbo",
                table: "Interventions",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Interventions_TeamMemberId",
                schema: "dbo",
                table: "Interventions",
                column: "TeamMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Interventions_Tenant_Date",
                schema: "dbo",
                table: "Interventions",
                columns: new[] { "TenantId", "StartDateTime" },
                filter: "[IsDeleted] = 0")
                .Annotation("SqlServer:Include", new[] { "ClientId", "TeamMemberId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Interventions_VehicleId",
                schema: "dbo",
                table: "Interventions",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "UQ_Resources_Name",
                schema: "dbo",
                table: "Resources",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_ActionId",
                schema: "dbo",
                table: "RolePermissions",
                column: "ActionId")
                .Annotation("SqlServer:Include", new[] { "TenantId", "RoleId", "ResourceId" });

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_ResourceId",
                schema: "dbo",
                table: "RolePermissions",
                column: "ResourceId")
                .Annotation("SqlServer:Include", new[] { "TenantId", "RoleId", "ActionId" });

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RoleId",
                schema: "dbo",
                table: "RolePermissions",
                column: "RoleId")
                .Annotation("SqlServer:Include", new[] { "TenantId", "ResourceId", "ActionId" });

            migrationBuilder.CreateIndex(
                name: "UQ_RolePermissions",
                schema: "dbo",
                table: "RolePermissions",
                columns: new[] { "TenantId", "RoleId", "ResourceId", "ActionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Roles_Tenant_Name",
                schema: "dbo",
                table: "Roles",
                columns: new[] { "TenantId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeamMemberAddresses_TeamMemberId",
                schema: "dbo",
                table: "TeamMemberAddresses",
                column: "TeamMemberId",
                filter: "[IsDeleted] = 0")
                .Annotation("SqlServer:Include", new[] { "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_TeamMemberAddresses_TenantId",
                schema: "dbo",
                table: "TeamMemberAddresses",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMemberContacts_TeamMemberId",
                schema: "dbo",
                table: "TeamMemberContacts",
                column: "TeamMemberId",
                filter: "[IsDeleted] = 0")
                .Annotation("SqlServer:Include", new[] { "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_TeamMemberContacts_TenantId",
                schema: "dbo",
                table: "TeamMemberContacts",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembers_TenantId",
                schema: "dbo",
                table: "TeamMembers",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantAddresses_TenantId",
                schema: "dbo",
                table: "TenantAddresses",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantContacts_TenantId",
                schema: "dbo",
                table: "TenantContacts",
                column: "TenantId",
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_TenantFiscalData_TenantId",
                schema: "dbo",
                table: "TenantFiscalData",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "UQ_TenantFiscalData_NIF",
                schema: "dbo",
                table: "TenantFiscalData",
                column: "NIF",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                schema: "dbo",
                table: "UserRoles",
                column: "RoleId")
                .Annotation("SqlServer:Include", new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId1",
                schema: "dbo",
                table: "UserRoles",
                column: "RoleId1");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                schema: "dbo",
                table: "UserRoles",
                column: "UserId")
                .Annotation("SqlServer:Include", new[] { "TenantId", "RoleId" });

            migrationBuilder.CreateIndex(
                name: "UQ_UserRoles",
                schema: "dbo",
                table: "UserRoles",
                columns: new[] { "TenantId", "UserId", "RoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Login",
                schema: "dbo",
                table: "Users",
                columns: new[] { "TenantId", "Email" },
                unique: true,
                filter: "[IsDeleted] = 0")
                .Annotation("SqlServer:Include", new[] { "Id", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "UQ_Vehicles_Tenant_Plate",
                schema: "dbo",
                table: "Vehicles",
                columns: new[] { "TenantId", "Plate" },
                unique: true);

            // ============================================
            // ROW LEVEL SECURITY (RLS)
            // ============================================
            // Aplica a função de acesso e política de segurança
            // conforme definido no arquivo docs/sql/Create-Tables.sql
            RowLevelSecurityMigration.ApplyRowLevelSecurity(migrationBuilder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // ============================================
            // ROW LEVEL SECURITY (RLS) - Remoção
            // ============================================
            // Remove a política de segurança e função de acesso
            RowLevelSecurityMigration.RemoveRowLevelSecurity(migrationBuilder);

            migrationBuilder.DropTable(
                name: "ClientAddresses",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ClientContacts",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ClientFiscalData",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Equipments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "InterventionAddresses",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "InterventionContacts",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RolePermissions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TeamMemberAddresses",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TeamMemberContacts",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TenantAddresses",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TenantContacts",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TenantFiscalData",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UserRoles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Interventions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Actions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Resources",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Clients",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TeamMembers",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Vehicles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Tenants",
                schema: "dbo");
        }
    }
}
