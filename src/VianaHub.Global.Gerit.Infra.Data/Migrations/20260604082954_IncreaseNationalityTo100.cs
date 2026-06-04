using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VianaHub.Global.Gerit.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class IncreaseNationalityTo100 : Migration
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
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: false),
                    ModifiedBy = table.Column<int>(type: "INT", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AddressTypes",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "INT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    ModifiedBy = table.Column<int>(type: "INT", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConsentTypes",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: false),
                    ModifiedBy = table.Column<int>(type: "INT", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileTypes",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MimeType = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    Extension = table.Column<string>(type: "NVARCHAR(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: false),
                    ModifiedBy = table.Column<int>(type: "INT", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobDefinitions",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobCategory = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    JobName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    JobPurpose = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    JobType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    JobMethod = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CronExpression = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TimeZoneId = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ExecuteOnlyOnce = table.Column<bool>(type: "bit", nullable: false),
                    TimeoutMinutes = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Queue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MaxRetries = table.Column<int>(type: "int", nullable: false),
                    JobConfiguration = table.Column<string>(type: "text", nullable: true),
                    IsSystemJob = table.Column<bool>(type: "bit", nullable: false),
                    HangfireJobId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LastRegisteredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: false),
                    ModifiedBy = table.Column<int>(type: "INT", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobDefinitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Plans",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PricePerHour = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    PricePerDay = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    PricePerMonth = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    PricePerYear = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false, defaultValue: "USD"),
                    MaxUsers = table.Column<int>(type: "int", nullable: false),
                    MaxPhotosPerVisits = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "INT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    ModifiedBy = table.Column<int>(type: "INT", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plans", x => x.Id);
                    table.CheckConstraint("CK_Plans_DeletedImpliesInactive", "[IsDeleted] = 0 OR [IsActive] = 0");
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RevokedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Resources",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR(255)", maxLength: 255, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: false),
                    ModifiedBy = table.Column<int>(type: "INT", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StatusTypes",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: false),
                    ModifiedBy = table.Column<int>(type: "INT", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantType = table.Column<int>(type: "int", nullable: false),
                    OriginType = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    Website = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    UrlImage = table.Column<string>(type: "NVARCHAR(500)", nullable: true),
                    Note = table.Column<string>(type: "NVARCHAR(1000)", nullable: true),
                    IsActive = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "INT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    ModifiedBy = table.Column<int>(type: "INT", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AttachmentCategories",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR(300)", maxLength: 300, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsSystem = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: false),
                    ModifiedBy = table.Column<int>(type: "INT", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentCategories", x => x.Id);
                    table.UniqueConstraint("UQ_AttachmentCategories_Id_Tenant", x => new { x.Id, x.TenantId });
                    table.CheckConstraint("CK_AttachmentCategories_Active_Deleted", "NOT ([IsActive] = 1 AND [IsDeleted] = 1)");
                    table.ForeignKey(
                        name: "FK_AttachmentCategories_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "INT", nullable: false),
                    ClientType = table.Column<int>(type: "INT", nullable: false),
                    OriginType = table.Column<int>(type: "INT", nullable: false),
                    UrlImage = table.Column<string>(type: "NVARCHAR(500)", maxLength: 500, nullable: true),
                    Note = table.Column<string>(type: "NVARCHAR(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: false),
                    ModifiedBy = table.Column<int>(type: "INT", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                    table.UniqueConstraint("UQ_Clients_Id_Tenant", x => new { x.Id, x.TenantId });
                    table.ForeignKey(
                        name: "FK_Clients_Tenant",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR(150)", maxLength: 150, nullable: false),
                    TaxNumber = table.Column<string>(type: "NVARCHAR(20)", maxLength: 20, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: false),
                    ModifiedBy = table.Column<int>(type: "INT", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.UniqueConstraint("UQ_Employees_Id_Tenant", x => new { x.Id, x.TenantId });
                    table.ForeignKey(
                        name: "FK_Employees_Tenant",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentTypes",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "INT", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR(200)", nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR(500)", nullable: false),
                    IsActive = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "INT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    ModifiedBy = table.Column<int>(type: "INT", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentTypes", x => x.Id);
                    table.UniqueConstraint("UQ_EquipmentTypes_Id_Tenant", x => new { x.Id, x.TenantId });
                    table.ForeignKey(
                        name: "FK_EquipmentTypes_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Functions",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR(255)", maxLength: 255, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: false),
                    ModifiedBy = table.Column<int>(type: "INT", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Functions", x => x.Id);
                    table.UniqueConstraint("UQ_Functions_Id_Tenant", x => new { x.Id, x.TenantId });
                    table.ForeignKey(
                        name: "FK_Functions_Tenant",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: false),
                    ModifiedBy = table.Column<int>(type: "INT", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "OriginTypes",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: false),
                    ModifiedBy = table.Column<int>(type: "INT", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OriginTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OriginTypes_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    Description = table.Column<string>(type: "NVARCHAR(255)", maxLength: 255, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: false),
                    ModifiedBy = table.Column<int>(type: "INT", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: true)
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
                name: "Status",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "INT", nullable: false),
                    StatusTypeId = table.Column<int>(type: "INT", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: false),
                    ModifiedBy = table.Column<int>(type: "INT", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Id);
                    table.UniqueConstraint("UQ_Status_Id_Tenant", x => new { x.Id, x.TenantId });
                    table.ForeignKey(
                        name: "FK_Status_StatusType",
                        column: x => x.StatusTypeId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    PlanId = table.Column<int>(type: "int", nullable: false),
                    StripeId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CurrentPeriodStart = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    CurrentPeriodEnd = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    TrialStart = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    TrialEnd = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    CancelAtPeriodEnd = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    CanceledAt = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    CancellationReason = table.Column<string>(type: "NVARCHAR(500)", maxLength: 500, nullable: true),
                    StripeCustomerId = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "INT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    ModifiedBy = table.Column<int>(type: "INT", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.CheckConstraint("CK_Subscriptions_DeletedImpliesInactive", "[IsDeleted] = 0 OR [IsActive] = 0");
                    table.ForeignKey(
                        name: "FK_Subscriptions_Plan",
                        column: x => x.PlanId,
                        principalSchema: "dbo",
                        principalTable: "Plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Tenant",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: false),
                    ModifiedBy = table.Column<int>(type: "INT", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                    table.UniqueConstraint("UQ_Teams_Id_Tenant", x => new { x.Id, x.TenantId });
                    table.ForeignKey(
                        name: "FK_Teams_Tenant",
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
                    TenantId = table.Column<int>(type: "INT", nullable: false),
                    AddressTypeId = table.Column<int>(type: "INT", nullable: false),
                    CountryCode = table.Column<string>(type: "CHAR(2)", maxLength: 2, nullable: false, defaultValue: "PT"),
                    Street = table.Column<string>(type: "NVARCHAR(200)", maxLength: 200, nullable: false),
                    Neighborhood = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    District = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: true),
                    PostalCode = table.Column<string>(type: "NVARCHAR(20)", maxLength: 20, nullable: false),
                    StreetNumber = table.Column<string>(type: "NVARCHAR(20)", maxLength: 20, nullable: true),
                    Complement = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: true),
                    Latitude = table.Column<decimal>(type: "DECIMAL(9,6)", nullable: true),
                    Longitude = table.Column<decimal>(type: "DECIMAL(9,6)", nullable: true),
                    Note = table.Column<string>(type: "NVARCHAR(500)", maxLength: 500, nullable: true),
                    IsPrimary = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "INT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    ModifiedBy = table.Column<int>(type: "INT", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenantAddresses_AddressTypes_AddressTypeId",
                        column: x => x.AddressTypeId,
                        principalSchema: "dbo",
                        principalTable: "AddressTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    CreatedBy = table.Column<int>(type: "INT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    ModifiedBy = table.Column<int>(type: "INT", nullable: true),
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
                    TenantId = table.Column<int>(type: "INT", nullable: false),
                    NIF = table.Column<string>(type: "CHAR(9)", maxLength: 9, nullable: false),
                    VATNumber = table.Column<string>(type: "NVARCHAR(20)", maxLength: 20, nullable: false),
                    CAE = table.Column<string>(type: "NVARCHAR(10)", maxLength: 10, nullable: true),
                    FiscalCountry = table.Column<string>(type: "CHAR(2)", maxLength: 2, nullable: false, defaultValue: "PT"),
                    IsVATRegistered = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    IsActive = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "INT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    ModifiedBy = table.Column<int>(type: "INT", nullable: true),
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
                    Name = table.Column<string>(type: "NVARCHAR(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR(256)", maxLength: 256, nullable: false),
                    NormalizedEmail = table.Column<string>(type: "NVARCHAR(256)", maxLength: 256, nullable: false),
                    EmailConfirmed = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    PhoneNumber = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    LastAccessAt = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    PasswordHash = table.Column<string>(type: "NVARCHAR(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: false),
                    ModifiedBy = table.Column<int>(type: "INT", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.UniqueConstraint("UQ_Users_Id_Tenant", x => new { x.Id, x.TenantId });
                    table.ForeignKey(
                        name: "FK_Users_Tenant",
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
                    TenantId = table.Column<int>(type: "INT", nullable: false),
                    ClientId = table.Column<int>(type: "INT", nullable: false),
                    AddressTypeId = table.Column<int>(type: "INT", nullable: false),
                    CountryCode = table.Column<string>(type: "CHAR(2)", maxLength: 2, nullable: false, defaultValue: "PT"),
                    Street = table.Column<string>(type: "NVARCHAR(200)", maxLength: 200, nullable: false),
                    StreetNumber = table.Column<string>(type: "NVARCHAR(20)", maxLength: 20, nullable: true),
                    Complement = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: true),
                    Neighborhood = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    District = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: true),
                    PostalCode = table.Column<string>(type: "NVARCHAR(20)", maxLength: 20, nullable: false),
                    Latitude = table.Column<decimal>(type: "DECIMAL(9,6)", nullable: true),
                    Longitude = table.Column<decimal>(type: "DECIMAL(9,6)", nullable: true),
                    Note = table.Column<string>(type: "NVARCHAR(500)", maxLength: 500, nullable: true),
                    IsPrimary = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "INT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    ModifiedBy = table.Column<int>(type: "INT", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientAddresses_AddressTypes_AddressTypeId",
                        column: x => x.AddressTypeId,
                        principalSchema: "dbo",
                        principalTable: "AddressTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientAddresses_Client",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClientCompanies",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "INT", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    LegalName = table.Column<string>(type: "NVARCHAR(200)", maxLength: 200, nullable: false),
                    TradeName = table.Column<string>(type: "NVARCHAR(200)", maxLength: 200, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CellPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsWhatsapp = table.Column<bool>(type: "bit", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Site = table.Column<string>(type: "NVARCHAR(500)", maxLength: 500, nullable: true),
                    CompanyRegistration = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    CAE = table.Column<string>(type: "NVARCHAR(10)", maxLength: 10, nullable: true),
                    NumberOfEmployee = table.Column<int>(type: "int", nullable: true),
                    LegalRepresentative = table.Column<string>(type: "NVARCHAR(150)", maxLength: 150, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: false),
                    ModifiedBy = table.Column<int>(type: "INT", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientCompanies", x => x.Id);
                    table.UniqueConstraint("UQ_ClientCompanies_Id_Tenant", x => new { x.Id, x.TenantId });
                    table.CheckConstraint("CK_ClientCompanies_Active_Deleted", "NOT ([IsActive] = 1 AND [IsDeleted] = 1)");
                    table.ForeignKey(
                        name: "FK_ClientCompanies_Client",
                        columns: x => new { x.ClientId, x.TenantId },
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumns: new[] { "Id", "TenantId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClientConsents",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "INT", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    ConsentTypeId = table.Column<int>(type: "int", nullable: false),
                    Granted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    GrantedDate = table.Column<DateTime>(type: "DATETIME2(7)", nullable: false),
                    RevokedDate = table.Column<DateTime>(type: "DATETIME2(7)", nullable: true),
                    Origin = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false),
                    IpAddress = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    UserAgent = table.Column<string>(type: "NVARCHAR(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: false),
                    ModifiedBy = table.Column<int>(type: "INT", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientConsents", x => x.Id);
                    table.UniqueConstraint("UQ_ClientConsents_Id_Tenant", x => new { x.Id, x.TenantId });
                    table.ForeignKey(
                        name: "FK_ClientConsents_Client",
                        columns: x => new { x.ClientId, x.TenantId },
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumns: new[] { "Id", "TenantId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientConsents_ConsentTypes_ConsentTypeId",
                        column: x => x.ConsentTypeId,
                        principalSchema: "dbo",
                        principalTable: "ConsentTypes",
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
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CellPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsWhatsapp = table.Column<bool>(type: "bit", nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR(255)", maxLength: 255, nullable: false),
                    IsPrimary = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: false),
                    ModifiedBy = table.Column<int>(type: "INT", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "ClientFiscalData",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "INT", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    TaxNumber = table.Column<string>(type: "NVARCHAR(20)", maxLength: 20, nullable: false),
                    VatNumber = table.Column<string>(type: "NVARCHAR(20)", maxLength: 20, nullable: true),
                    FiscalCountry = table.Column<string>(type: "CHAR(2)", maxLength: 2, nullable: false, defaultValue: "PT"),
                    IsVatRegistered = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IBAN = table.Column<string>(type: "NVARCHAR(34)", maxLength: 34, nullable: true),
                    FiscalEmail = table.Column<string>(type: "NVARCHAR(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: false),
                    ModifiedBy = table.Column<int>(type: "INT", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientFiscalData", x => x.Id);
                    table.CheckConstraint("CK_ClientFiscalData_Active_Deleted", "NOT ([IsActive] = 1 AND [IsDeleted] = 1)");
                    table.ForeignKey(
                        name: "FK_ClientFiscalData_Client",
                        columns: x => new { x.ClientId, x.TenantId },
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumns: new[] { "Id", "TenantId" },
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
                name: "ClientHierarchies",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "INT", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    ChildId = table.Column<int>(type: "int", nullable: false),
                    RelationshipType = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: false),
                    ModifiedBy = table.Column<int>(type: "INT", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientHierarchies", x => x.Id);
                    table.UniqueConstraint("UQ_ClientHierarchies_Id_Tenant", x => new { x.Id, x.TenantId });
                    table.ForeignKey(
                        name: "FK_ClientHierarchies_ChildClient",
                        columns: x => new { x.ChildId, x.TenantId },
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumns: new[] { "Id", "TenantId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientHierarchies_ParentClient",
                        columns: x => new { x.ParentId, x.TenantId },
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumns: new[] { "Id", "TenantId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClientIndividuals",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "INT", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false),
                    CellPhoneNumber = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    IsWhatsapp = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Email = table.Column<string>(type: "NVARCHAR(500)", maxLength: 100, nullable: true),
                    BirthDate = table.Column<DateTime>(type: "DATE", nullable: false),
                    Gender = table.Column<string>(type: "NVARCHAR(20)", maxLength: 20, nullable: true),
                    DocumentType = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    DocumentNumber = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    Nationality = table.Column<string>(type: "CHAR(2)", maxLength: 2, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: false),
                    ModifiedBy = table.Column<int>(type: "INT", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientIndividuals", x => x.Id);
                    table.UniqueConstraint("UQ_ClientIndividuals_Id_Tenant", x => new { x.Id, x.TenantId });
                    table.CheckConstraint("CK_ClientIndividuals_Active_Deleted", "NOT ([IsActive] = 1 AND [IsDeleted] = 1)");
                    table.ForeignKey(
                        name: "FK_ClientIndividuals_Client",
                        columns: x => new { x.ClientId, x.TenantId },
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumns: new[] { "Id", "TenantId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeAddresses",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "INT", nullable: false),
                    EmployeeId = table.Column<int>(type: "INT", nullable: false),
                    AddressTypeId = table.Column<int>(type: "INT", nullable: false),
                    CountryCode = table.Column<string>(type: "CHAR(2)", maxLength: 2, nullable: false, defaultValue: "PT"),
                    Street = table.Column<string>(type: "NVARCHAR(200)", maxLength: 200, nullable: false),
                    Neighborhood = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    District = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: true),
                    PostalCode = table.Column<string>(type: "NVARCHAR(20)", maxLength: 20, nullable: false),
                    StreetNumber = table.Column<string>(type: "NVARCHAR(20)", maxLength: 20, nullable: true),
                    Complement = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: true),
                    Latitude = table.Column<decimal>(type: "DECIMAL(9,6)", nullable: true),
                    Longitude = table.Column<decimal>(type: "DECIMAL(9,6)", nullable: true),
                    Note = table.Column<string>(type: "NVARCHAR(500)", maxLength: 500, nullable: true),
                    IsPrimary = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "INT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    ModifiedBy = table.Column<int>(type: "INT", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeAddresses_AddressTypes_AddressTypeId",
                        column: x => x.AddressTypeId,
                        principalSchema: "dbo",
                        principalTable: "AddressTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeAddresses_Employee",
                        column: x => x.EmployeeId,
                        principalSchema: "dbo",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeAddresses_Tenant",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmployeeContacts",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "NVARCHAR(30)", maxLength: 30, nullable: true),
                    IsPrimary = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: false),
                    ModifiedBy = table.Column<int>(type: "INT", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeContacts_Employee",
                        column: x => x.EmployeeId,
                        principalSchema: "dbo",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeContacts_Tenant",
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
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ResourceId = table.Column<int>(type: "int", nullable: false),
                    ActionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.TenantId, x.RoleId, x.ResourceId, x.ActionId });
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
                name: "Equipments",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "INT", nullable: false),
                    EquipmentTypeId = table.Column<int>(type: "INT", nullable: false),
                    StatusId = table.Column<int>(type: "INT", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR(150)", maxLength: 150, nullable: false),
                    SerialNumber = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: false),
                    ModifiedBy = table.Column<int>(type: "INT", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Equipments_EquipamentType",
                        columns: x => new { x.EquipmentTypeId, x.TenantId },
                        principalSchema: "dbo",
                        principalTable: "EquipmentTypes",
                        principalColumns: new[] { "Id", "TenantId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Equipments_Status",
                        columns: x => new { x.StatusId, x.TenantId },
                        principalSchema: "dbo",
                        principalTable: "Status",
                        principalColumns: new[] { "Id", "TenantId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Equipments_Tenant",
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
                    StatusId = table.Column<int>(type: "INT", nullable: false),
                    Plate = table.Column<string>(type: "NVARCHAR(20)", maxLength: 20, nullable: false),
                    Brand = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    Model = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    FuelType = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: false),
                    ModifiedBy = table.Column<int>(type: "INT", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.UniqueConstraint("UQ_Vehicles_Id_Tenant", x => new { x.Id, x.TenantId });
                    table.ForeignKey(
                        name: "FK_Vehicles_Status",
                        column: x => x.StatusId,
                        principalSchema: "dbo",
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vehicles_Tenants",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Visits",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "INT", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "INT", nullable: false),
                    Title = table.Column<string>(type: "NVARCHAR(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR(2000)", maxLength: 2000, nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    EstimatedValue = table.Column<decimal>(type: "DECIMAL(10,2)", precision: 10, scale: 2, nullable: false),
                    RealValue = table.Column<decimal>(type: "DECIMAL(10,2)", precision: 10, scale: 2, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: false),
                    ModifiedBy = table.Column<int>(type: "INT", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visits", x => x.Id);
                    table.UniqueConstraint("AK_Visits_Id_TenantId", x => new { x.Id, x.TenantId });
                    table.CheckConstraint("CK_Visits_EndDateTime", "[EndDateTime] IS NULL OR [EndDateTime] >= [StartDateTime]");
                    table.ForeignKey(
                        name: "FK_Visits_Clients",
                        columns: x => new { x.ClientId, x.TenantId },
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumns: new[] { "Id", "TenantId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Visits_Status",
                        columns: x => new { x.StatusId, x.TenantId },
                        principalSchema: "dbo",
                        principalTable: "Status",
                        principalColumns: new[] { "Id", "TenantId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Visits_Tenants",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeTeams",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "INT", nullable: false),
                    TeamId = table.Column<int>(type: "INT", nullable: false),
                    EmployeeId = table.Column<int>(type: "INT", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: false),
                    ModifiedBy = table.Column<int>(type: "INT", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeTeams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeAddresses_Tenant",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployeeTeams_Employee",
                        column: x => x.EmployeeId,
                        principalSchema: "dbo",
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployeeTeams_Team",
                        column: x => x.TeamId,
                        principalSchema: "dbo",
                        principalTable: "Teams",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserPreferences",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Appearance = table.Column<string>(type: "NVARCHAR(10)", maxLength: 10, nullable: false, defaultValue: "light"),
                    CurrencyCode = table.Column<string>(type: "NVARCHAR(3)", maxLength: 3, nullable: false, defaultValue: "EUR"),
                    Locale = table.Column<string>(type: "NVARCHAR(10)", maxLength: 10, nullable: false, defaultValue: "pt-PT"),
                    Timezone = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false, defaultValue: "Europe/Lisbon"),
                    DateFormat = table.Column<string>(type: "NVARCHAR(20)", maxLength: 20, nullable: false, defaultValue: "DD-MM-YYYY"),
                    TimeFormat = table.Column<string>(type: "NVARCHAR(10)", maxLength: 10, nullable: false, defaultValue: "24h"),
                    DayStart = table.Column<TimeSpan>(type: "TIME(0)", nullable: false, defaultValueSql: "('09:00')"),
                    DayEnd = table.Column<TimeSpan>(type: "TIME(0)", nullable: false, defaultValueSql: "('18:00')"),
                    EmailNewsletter = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    EmailWeeklyReport = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    EmailApproval = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    EmailAlerts = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    EmailReminders = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    EmailPlanner = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: false),
                    ModifiedBy = table.Column<int>(type: "INT", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPreferences", x => x.Id);
                    table.UniqueConstraint("UQ_UserPreferences_Id_Tenant", x => new { x.Id, x.TenantId });
                    table.CheckConstraint("CK_UserPreferences_Active_Deleted", "NOT (IsActive = 1 AND IsDeleted = 1)");
                    table.CheckConstraint("CK_UserPreferences_TimeFormat", "TimeFormat IN ('24h', '12h')");
                    table.ForeignKey(
                        name: "FK_UserPreferences_Tenant",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserPreferences_User",
                        columns: x => new { x.UserId, x.TenantId },
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumns: new[] { "Id", "TenantId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                schema: "dbo",
                columns: table => new
                {
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.TenantId, x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Role",
                        column: x => x.RoleId,
                        principalSchema: "dbo",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "VisitAddresses",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "INT", nullable: false),
                    VisitId = table.Column<int>(type: "INT", nullable: false),
                    AddressTypeId = table.Column<int>(type: "INT", nullable: false),
                    CountryCode = table.Column<string>(type: "CHAR(2)", maxLength: 2, nullable: false, defaultValue: "PT"),
                    Street = table.Column<string>(type: "NVARCHAR(200)", maxLength: 200, nullable: false),
                    StreetNumber = table.Column<string>(type: "NVARCHAR(20)", maxLength: 20, nullable: true),
                    Complement = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: true),
                    Neighborhood = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    District = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: true),
                    PostalCode = table.Column<string>(type: "NVARCHAR(20)", maxLength: 20, nullable: false),
                    Latitude = table.Column<decimal>(type: "DECIMAL(9,6)", nullable: true),
                    Longitude = table.Column<decimal>(type: "DECIMAL(9,6)", nullable: true),
                    Note = table.Column<string>(type: "NVARCHAR(500)", maxLength: 500, nullable: true),
                    IsPrimary = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: false),
                    ModifiedBy = table.Column<int>(type: "INT", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VisitAddresses_AddressTypes_AddressTypeId",
                        column: x => x.AddressTypeId,
                        principalSchema: "dbo",
                        principalTable: "AddressTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VisitAddresses_Tenant",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VisitAddresses_Visit",
                        column: x => x.VisitId,
                        principalSchema: "dbo",
                        principalTable: "Visits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VisitAttachments",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "INT", nullable: false),
                    FileTypeId = table.Column<int>(type: "int", nullable: false),
                    VisitId = table.Column<int>(type: "int", nullable: false),
                    AttachmentCategoryId = table.Column<int>(type: "int", nullable: false),
                    PublicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    S3Key = table.Column<string>(type: "NVARCHAR(500)", maxLength: 500, nullable: false),
                    FileName = table.Column<string>(type: "NVARCHAR(255)", maxLength: 255, nullable: false),
                    FileSizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: false),
                    ModifiedBy = table.Column<int>(type: "INT", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitAttachments", x => x.Id);
                    table.UniqueConstraint("UQ_VisitAttachments_Id_Tenant", x => new { x.Id, x.TenantId });
                    table.CheckConstraint("CK_VisitAttachments_Active_Deleted", "NOT ([IsActive] = 1 AND [IsDeleted] = 1)");
                    table.CheckConstraint("CK_VisitAttachments_FileSizeBytes", "[FileSizeBytes] > 0");
                    table.ForeignKey(
                        name: "FK_VisitAttachments_AttachmentCategories_AttachmentCategoryId_TenantId",
                        columns: x => new { x.AttachmentCategoryId, x.TenantId },
                        principalSchema: "dbo",
                        principalTable: "AttachmentCategories",
                        principalColumns: new[] { "Id", "TenantId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VisitAttachments_FileTypes_FileTypeId",
                        column: x => x.FileTypeId,
                        principalSchema: "dbo",
                        principalTable: "FileTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VisitAttachments_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VisitAttachments_Visits_VisitId_TenantId",
                        columns: x => new { x.VisitId, x.TenantId },
                        principalSchema: "dbo",
                        principalTable: "Visits",
                        principalColumns: new[] { "Id", "TenantId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VisitContacts",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    VisitId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "NVARCHAR(30)", maxLength: 30, nullable: true),
                    IsPrimary = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: false),
                    ModifiedBy = table.Column<int>(type: "INT", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VisitContacts_Tenant",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VisitContacts_Visit",
                        column: x => x.VisitId,
                        principalSchema: "dbo",
                        principalTable: "Visits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VisitTeams",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    VisitId = table.Column<int>(type: "int", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitTeams", x => x.Id);
                    table.UniqueConstraint("UQ_VisitTeam_Id_Tenant", x => new { x.Id, x.TenantId });
                    table.ForeignKey(
                        name: "FK_VisitTeams_Teams_TeamId",
                        column: x => x.TeamId,
                        principalSchema: "dbo",
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VisitTeams_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VisitTeams_Visits_VisitId",
                        column: x => x.VisitId,
                        principalSchema: "dbo",
                        principalTable: "Visits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VisitTeamEmployee",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    VisitTeamId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    FunctionId = table.Column<int>(type: "int", nullable: false),
                    IsLeader = table.Column<bool>(type: "bit", nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: false),
                    ModifiedBy = table.Column<int>(type: "INT", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitTeamEmployee", x => x.Id);
                    table.CheckConstraint("CK_VisitTeamEmployee_Active_Deleted", "NOT ([IsActive] = 1 AND [IsDeleted] = 1)");
                    table.CheckConstraint("CK_VisitTeamEmployee_EndDateTime", "[EndDateTime] IS NULL OR [EndDateTime] >= [StartDateTime]");
                    table.ForeignKey(
                        name: "FK_VisitTeamEmployee_Employees_EmployeeId_TenantId",
                        columns: x => new { x.EmployeeId, x.TenantId },
                        principalSchema: "dbo",
                        principalTable: "Employees",
                        principalColumns: new[] { "Id", "TenantId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VisitTeamEmployee_Functions_FunctionId_TenantId",
                        columns: x => new { x.FunctionId, x.TenantId },
                        principalSchema: "dbo",
                        principalTable: "Functions",
                        principalColumns: new[] { "Id", "TenantId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VisitTeamEmployee_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VisitTeamEmployee_VisitTeams_VisitTeamId_TenantId",
                        columns: x => new { x.VisitTeamId, x.TenantId },
                        principalSchema: "dbo",
                        principalTable: "VisitTeams",
                        principalColumns: new[] { "Id", "TenantId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VisitTeamEquipments",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    VisitTeamId = table.Column<int>(type: "int", nullable: false),
                    EquipmentId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitTeamEquipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VisitTeamEquipments_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalSchema: "dbo",
                        principalTable: "Equipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VisitTeamEquipments_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VisitTeamEquipments_VisitTeams_VisitTeamId",
                        column: x => x.VisitTeamId,
                        principalSchema: "dbo",
                        principalTable: "VisitTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VisitTeamVehicles",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    VisitTeamId = table.Column<int>(type: "int", nullable: false),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitTeamVehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VisitTeamVehicles_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VisitTeamVehicles_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalSchema: "dbo",
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VisitTeamVehicles_VisitTeams_VisitTeamId",
                        column: x => x.VisitTeamId,
                        principalSchema: "dbo",
                        principalTable: "VisitTeams",
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
                name: "IX_AttachmentCategories_Tenant_Active",
                schema: "dbo",
                table: "AttachmentCategories",
                column: "TenantId",
                filter: "[IsDeleted] = 0 AND [IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentCategories_Tenant_Display",
                schema: "dbo",
                table: "AttachmentCategories",
                columns: new[] { "TenantId", "DisplayOrder" },
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "UQ_AttachmentCategories_Name_Tenant",
                schema: "dbo",
                table: "AttachmentCategories",
                columns: new[] { "TenantId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientAddresses_AddressTypeId",
                schema: "dbo",
                table: "ClientAddresses",
                column: "AddressTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientAddresses_ClientId",
                schema: "dbo",
                table: "ClientAddresses",
                column: "ClientId",
                unique: true,
                filter: "[IsDeleted] = 0")
                .Annotation("SqlServer:Include", new[] { "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ClientCompanies_ClientId_TenantId",
                schema: "dbo",
                table: "ClientCompanies",
                columns: new[] { "ClientId", "TenantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_ClientCompanies_Client",
                schema: "dbo",
                table: "ClientCompanies",
                columns: new[] { "TenantId", "ClientId" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_ClientConsents_Client_ConsentType_Tenant",
                schema: "dbo",
                table: "ClientConsents",
                columns: new[] { "ClientId", "ConsentTypeId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ClientConsents_ClientId_TenantId",
                schema: "dbo",
                table: "ClientConsents",
                columns: new[] { "ClientId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ClientConsents_ConsentTypeId",
                schema: "dbo",
                table: "ClientConsents",
                column: "ConsentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientConsents_Granted_Tenant",
                schema: "dbo",
                table: "ClientConsents",
                columns: new[] { "Granted", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ClientConsents_TenantId",
                schema: "dbo",
                table: "ClientConsents",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientContacts_ClientId",
                schema: "dbo",
                table: "ClientContacts",
                column: "ClientId",
                filter: "[IsDeleted] = 0")
                .Annotation("SqlServer:Include", new[] { "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ClientFiscalData_TenantId",
                schema: "dbo",
                table: "ClientFiscalData",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "UQ_ClientFiscalData_Client",
                schema: "dbo",
                table: "ClientFiscalData",
                columns: new[] { "ClientId", "TenantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientHierarchies_ChildId_TenantId",
                schema: "dbo",
                table: "ClientHierarchies",
                columns: new[] { "ChildId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ClientHierarchies_ParentChild_Tenant",
                schema: "dbo",
                table: "ClientHierarchies",
                columns: new[] { "ParentId", "ChildId", "TenantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientHierarchies_ParentId_TenantId",
                schema: "dbo",
                table: "ClientHierarchies",
                columns: new[] { "ParentId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ClientHierarchies_TenantId",
                schema: "dbo",
                table: "ClientHierarchies",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientIndividuals_ClientId_TenantId",
                schema: "dbo",
                table: "ClientIndividuals",
                columns: new[] { "ClientId", "TenantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_ClientIndividuals_Client",
                schema: "dbo",
                table: "ClientIndividuals",
                columns: new[] { "TenantId", "ClientId" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "UX_ClientIndividuals_Document",
                schema: "dbo",
                table: "ClientIndividuals",
                columns: new[] { "TenantId", "DocumentType", "DocumentNumber" },
                unique: true,
                filter: "[DocumentNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_TenantId",
                schema: "dbo",
                table: "Clients",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAddresses_AddressTypeId",
                schema: "dbo",
                table: "EmployeeAddresses",
                column: "AddressTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAddresses_EmployeeId",
                schema: "dbo",
                table: "EmployeeAddresses",
                column: "EmployeeId",
                filter: "[IsDeleted] = 0")
                .Annotation("SqlServer:Include", new[] { "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAddresses_TenantId",
                schema: "dbo",
                table: "EmployeeAddresses",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeContacts_EmployeeId",
                schema: "dbo",
                table: "EmployeeContacts",
                column: "EmployeeId",
                filter: "[IsDeleted] = 0")
                .Annotation("SqlServer:Include", new[] { "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeContacts_TenantId",
                schema: "dbo",
                table: "EmployeeContacts",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_TenantId",
                schema: "dbo",
                table: "Employees",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTeams_EmployeeId",
                schema: "dbo",
                table: "EmployeeTeams",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTeams_TeamId",
                schema: "dbo",
                table: "EmployeeTeams",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTeams_TenantId",
                schema: "dbo",
                table: "EmployeeTeams",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_EquipmentTypeId_TenantId",
                schema: "dbo",
                table: "Equipments",
                columns: new[] { "EquipmentTypeId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_StatusId_TenantId",
                schema: "dbo",
                table: "Equipments",
                columns: new[] { "StatusId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_TenantId",
                schema: "dbo",
                table: "Equipments",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentTypes_TenantId_Name",
                schema: "dbo",
                table: "EquipmentTypes",
                columns: new[] { "TenantId", "Name" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "UQ_FileTypes_Mime",
                schema: "dbo",
                table: "FileTypes",
                column: "MimeType",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Functions_TenantId",
                schema: "dbo",
                table: "Functions",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_Active_SYSTEM",
                schema: "dbo",
                table: "JobDefinitions",
                columns: new[] { "IsActive", "IsSystemJob" },
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Services_Category_Active",
                schema: "dbo",
                table: "JobDefinitions",
                columns: new[] { "JobCategory", "IsActive", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Services_HangfireJobId",
                schema: "dbo",
                table: "JobDefinitions",
                column: "HangfireJobId",
                filter: "HangfireJobId IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ_Job_JobName",
                schema: "dbo",
                table: "JobDefinitions",
                column: "JobName",
                unique: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_OriginTypes_TenantId",
                schema: "dbo",
                table: "OriginTypes",
                column: "TenantId");

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
                name: "IX_Status_StatusTypeId",
                schema: "dbo",
                table: "Status",
                column: "StatusTypeId");

            migrationBuilder.CreateIndex(
                name: "UQ_Status_Tenant_Name",
                schema: "dbo",
                table: "Status",
                columns: new[] { "TenantId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_PlanId",
                schema: "dbo",
                table: "Subscriptions",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "UQ_Subscriptions_Tenant_Active",
                schema: "dbo",
                table: "Subscriptions",
                columns: new[] { "TenantId", "IsActive" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Subscriptions_TenantId_Id",
                schema: "dbo",
                table: "Subscriptions",
                columns: new[] { "TenantId", "Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_TenantId",
                schema: "dbo",
                table: "Teams",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantAddresses_AddressTypeId",
                schema: "dbo",
                table: "TenantAddresses",
                column: "AddressTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantAddresses_TenantId",
                schema: "dbo",
                table: "TenantAddresses",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "UQ_TenantAddresses_Id_Tenant",
                schema: "dbo",
                table: "TenantAddresses",
                columns: new[] { "Id", "TenantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TenantContacts_TenantId",
                schema: "dbo",
                table: "TenantContacts",
                column: "TenantId",
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "UQ_TenantFiscalData_NIF",
                schema: "dbo",
                table: "TenantFiscalData",
                column: "NIF",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_TenantFiscalData_Tenant_Active",
                schema: "dbo",
                table: "TenantFiscalData",
                columns: new[] { "TenantId", "IsActive" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPreferences_UserId_TenantId",
                schema: "dbo",
                table: "UserPreferences",
                columns: new[] { "UserId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "UX_UserPreferences_Tenant_User_Active",
                schema: "dbo",
                table: "UserPreferences",
                columns: new[] { "TenantId", "UserId" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                schema: "dbo",
                table: "UserRoles",
                column: "RoleId")
                .Annotation("SqlServer:Include", new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                schema: "dbo",
                table: "UserRoles",
                column: "UserId")
                .Annotation("SqlServer:Include", new[] { "TenantId", "RoleId" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Login",
                schema: "dbo",
                table: "Users",
                columns: new[] { "TenantId", "Email" },
                unique: true,
                filter: "[IsDeleted] = 0")
                .Annotation("SqlServer:Include", new[] { "Id", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "UQ_Users_Tenant_NormalizedEmail",
                schema: "dbo",
                table: "Users",
                columns: new[] { "TenantId", "NormalizedEmail" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_StatusId",
                schema: "dbo",
                table: "Vehicles",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "UQ_Vehicles_Tenant_Plate",
                schema: "dbo",
                table: "Vehicles",
                columns: new[] { "TenantId", "Plate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VisitAddresses_AddressTypeId",
                schema: "dbo",
                table: "VisitAddresses",
                column: "AddressTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitAddresses_TenantId",
                schema: "dbo",
                table: "VisitAddresses",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitAddresses_VisitId",
                schema: "dbo",
                table: "VisitAddresses",
                column: "VisitId",
                filter: "[IsDeleted] = 0")
                .Annotation("SqlServer:Include", new[] { "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_VisitAttachments_AttachmentCategoryId_TenantId",
                schema: "dbo",
                table: "VisitAttachments",
                columns: new[] { "AttachmentCategoryId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_VisitAttachments_FileTypeId",
                schema: "dbo",
                table: "VisitAttachments",
                columns: new[] { "TenantId", "FileTypeId" },
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_VisitAttachments_FileTypeId1",
                schema: "dbo",
                table: "VisitAttachments",
                column: "FileTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitAttachments_VisitId_TenantId",
                schema: "dbo",
                table: "VisitAttachments",
                columns: new[] { "VisitId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "UX_VisitAttachments_Primary",
                schema: "dbo",
                table: "VisitAttachments",
                columns: new[] { "TenantId", "VisitId" },
                unique: true,
                filter: "[IsPrimary] = 1 AND [IsDeleted] = 0 AND [IsActive] = 1")
                .Annotation("SqlServer:Include", new[] { "AttachmentCategoryId", "DisplayOrder", "IsPrimary", "FileTypeId" });

            migrationBuilder.CreateIndex(
                name: "UX_VisitAttachments_PublicId",
                schema: "dbo",
                table: "VisitAttachments",
                columns: new[] { "TenantId", "PublicId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_VisitAttachments_S3Key",
                schema: "dbo",
                table: "VisitAttachments",
                columns: new[] { "TenantId", "S3Key" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_VisitContacts_TenantId",
                schema: "dbo",
                table: "VisitContacts",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitContacts_VisitId",
                schema: "dbo",
                table: "VisitContacts",
                column: "VisitId",
                filter: "[IsDeleted] = 0")
                .Annotation("SqlServer:Include", new[] { "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_Visits_ClientId_TenantId",
                schema: "dbo",
                table: "Visits",
                columns: new[] { "ClientId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_Visits_StatusId_TenantId",
                schema: "dbo",
                table: "Visits",
                columns: new[] { "StatusId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_Visits_TenantId",
                schema: "dbo",
                table: "Visits",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "UQ_Visits_Id_Tenant",
                schema: "dbo",
                table: "Visits",
                columns: new[] { "Id", "TenantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VisitTeamEmployee_EmployeeId",
                schema: "dbo",
                table: "VisitTeamEmployee",
                columns: new[] { "TenantId", "EmployeeId" },
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_VisitTeamEmployee_EmployeeId_TenantId",
                schema: "dbo",
                table: "VisitTeamEmployee",
                columns: new[] { "EmployeeId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_VisitTeamEmployee_FunctionId_TenantId",
                schema: "dbo",
                table: "VisitTeamEmployee",
                columns: new[] { "FunctionId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_VisitTeamEmployee_VisitTeamId",
                schema: "dbo",
                table: "VisitTeamEmployee",
                columns: new[] { "TenantId", "VisitTeamId" },
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_VisitTeamEmployee_VisitTeamId_TenantId",
                schema: "dbo",
                table: "VisitTeamEmployee",
                columns: new[] { "VisitTeamId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "UQ_VisitTeamEmployee_Id_Tenant",
                schema: "dbo",
                table: "VisitTeamEmployee",
                columns: new[] { "Id", "TenantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_VisitTeamEmployee_Active",
                schema: "dbo",
                table: "VisitTeamEmployee",
                columns: new[] { "TenantId", "VisitTeamId", "EmployeeId" },
                unique: true,
                filter: "[EndDateTime] IS NULL AND [IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_VisitTeamEquipments_EquipmentId",
                schema: "dbo",
                table: "VisitTeamEquipments",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitTeamEquipments_TenantId_VisitTeamId_EquipmentId",
                schema: "dbo",
                table: "VisitTeamEquipments",
                columns: new[] { "TenantId", "VisitTeamId", "EquipmentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VisitTeamEquipments_VisitTeamId",
                schema: "dbo",
                table: "VisitTeamEquipments",
                column: "VisitTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitTeams_TeamId",
                schema: "dbo",
                table: "VisitTeams",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitTeams_TenantId_VisitId_TeamId",
                schema: "dbo",
                table: "VisitTeams",
                columns: new[] { "TenantId", "VisitId", "TeamId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VisitTeams_VisitId",
                schema: "dbo",
                table: "VisitTeams",
                column: "VisitId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitTeamVehicles_TenantId_VisitTeamId_VehicleId",
                schema: "dbo",
                table: "VisitTeamVehicles",
                columns: new[] { "TenantId", "VisitTeamId", "VehicleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VisitTeamVehicles_VehicleId",
                schema: "dbo",
                table: "VisitTeamVehicles",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitTeamVehicles_VisitTeamId",
                schema: "dbo",
                table: "VisitTeamVehicles",
                column: "VisitTeamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientAddresses",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ClientCompanies",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ClientConsents",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ClientContacts",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ClientFiscalData",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ClientHierarchies",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ClientIndividuals",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "EmployeeAddresses",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "EmployeeContacts",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "EmployeeTeams",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "JobDefinitions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "JwtKeys",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "OriginTypes",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RefreshTokens",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RolePermissions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "StatusTypes",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Subscriptions",
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
                name: "UserPreferences",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UserRoles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VisitAddresses",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VisitAttachments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VisitContacts",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VisitTeamEmployee",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VisitTeamEquipments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VisitTeamVehicles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ConsentTypes",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Actions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Resources",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Plans",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AddressTypes",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AttachmentCategories",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FileTypes",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Employees",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Functions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Equipments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Vehicles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VisitTeams",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "EquipmentTypes",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Teams",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Visits",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Clients",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Status",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Tenants",
                schema: "dbo");
        }
    }
}
