IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [dbo].[Actions] (
    [Id] int NOT NULL IDENTITY,
    [Name] NVARCHAR(50) NOT NULL,
    [IsActive] BIT NOT NULL DEFAULT CAST(1 AS BIT),
    [IsDeleted] BIT NOT NULL DEFAULT CAST(0 AS BIT),
    [CreatedBy] NVARCHAR(50) NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT (SYSDATETIME()),
    [ModifiedBy] NVARCHAR(50) NULL,
    [ModifiedAt] DATETIME2 NULL,
    CONSTRAINT [PK_Actions] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [dbo].[Resources] (
    [Id] int NOT NULL IDENTITY,
    [Name] NVARCHAR(100) NOT NULL,
    [IsActive] BIT NOT NULL DEFAULT CAST(1 AS BIT),
    [IsDeleted] BIT NOT NULL DEFAULT CAST(0 AS BIT),
    [CreatedBy] NVARCHAR(50) NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT (SYSDATETIME()),
    [ModifiedBy] NVARCHAR(50) NULL,
    [ModifiedAt] DATETIME2 NULL,
    CONSTRAINT [PK_Resources] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [dbo].[Tenants] (
    [Id] int NOT NULL IDENTITY,
    [LegalName] NVARCHAR(200) NOT NULL,
    [TradeName] NVARCHAR(200) NULL,
    [Consent] BIT NOT NULL DEFAULT CAST(1 AS BIT),
    [IsActive] BIT NOT NULL DEFAULT CAST(1 AS BIT),
    [IsDeleted] BIT NOT NULL DEFAULT CAST(0 AS BIT),
    [CreatedBy] NVARCHAR(50) NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT (SYSDATETIME()),
    [ModifiedBy] NVARCHAR(50) NULL,
    [ModifiedAt] DATETIME2 NULL,
    CONSTRAINT [PK_Tenants] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [dbo].[Clients] (
    [Id] int NOT NULL IDENTITY,
    [TenantId] int NOT NULL,
    [Name] NVARCHAR(150) NOT NULL,
    [Email] NVARCHAR(255) NULL,
    [Phone] NVARCHAR(50) NOT NULL,
    [Consent] BIT NOT NULL DEFAULT CAST(1 AS BIT),
    [IsActive] BIT NOT NULL DEFAULT CAST(1 AS BIT),
    [IsDeleted] BIT NOT NULL DEFAULT CAST(0 AS BIT),
    [CreatedBy] NVARCHAR(50) NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT (SYSDATETIME()),
    [ModifiedBy] NVARCHAR(50) NULL,
    [ModifiedAt] DATETIME2 NULL,
    CONSTRAINT [PK_Clients] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Clients_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenants] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [dbo].[Equipments] (
    [Id] int NOT NULL IDENTITY,
    [TenantId] int NOT NULL,
    [Name] NVARCHAR(150) NOT NULL,
    [SerialNumber] NVARCHAR(100) NULL,
    [IsActive] BIT NOT NULL DEFAULT CAST(1 AS BIT),
    [IsDeleted] BIT NOT NULL DEFAULT CAST(0 AS BIT),
    [CreatedBy] NVARCHAR(50) NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT (SYSDATETIME()),
    [ModifiedBy] NVARCHAR(50) NULL,
    [ModifiedAt] DATETIME2 NULL,
    CONSTRAINT [PK_Equipments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Equipments_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenants] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [dbo].[Roles] (
    [Id] int NOT NULL IDENTITY,
    [TenantId] int NOT NULL,
    [Name] NVARCHAR(100) NOT NULL,
    [IsActive] BIT NOT NULL DEFAULT CAST(1 AS BIT),
    [IsDeleted] BIT NOT NULL DEFAULT CAST(0 AS BIT),
    [CreatedBy] NVARCHAR(50) NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT (SYSDATETIME()),
    [ModifiedBy] NVARCHAR(50) NULL,
    [ModifiedAt] DATETIME2 NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Roles_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenants] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [dbo].[TeamMembers] (
    [Id] int NOT NULL IDENTITY,
    [TenantId] int NOT NULL,
    [Name] NVARCHAR(150) NOT NULL,
    [TaxNumber] NVARCHAR(20) NULL,
    [IsActive] BIT NOT NULL DEFAULT CAST(1 AS BIT),
    [IsDeleted] BIT NOT NULL DEFAULT CAST(0 AS BIT),
    [CreatedBy] NVARCHAR(50) NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT (SYSDATETIME()),
    [ModifiedBy] NVARCHAR(50) NULL,
    [ModifiedAt] DATETIME2 NULL,
    CONSTRAINT [PK_TeamMembers] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TeamMembers_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenants] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [dbo].[TenantAddresses] (
    [Id] int NOT NULL IDENTITY,
    [TenantId] int NOT NULL,
    [Street] NVARCHAR(200) NOT NULL,
    [City] NVARCHAR(100) NOT NULL,
    [PostalCode] NVARCHAR(20) NOT NULL,
    [District] NVARCHAR(100) NULL,
    [CountryCode] CHAR(2) NOT NULL DEFAULT 'PT',
    [IsPrimary] BIT NOT NULL DEFAULT CAST(0 AS BIT),
    [IsActive] BIT NOT NULL DEFAULT CAST(1 AS BIT),
    [IsDeleted] BIT NOT NULL DEFAULT CAST(0 AS BIT),
    [CreatedBy] NVARCHAR(50) NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT (SYSDATETIME()),
    [ModifiedBy] NVARCHAR(50) NULL,
    [ModifiedAt] DATETIME2 NULL,
    CONSTRAINT [PK_TenantAddresses] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TenantAddresses_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenants] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [dbo].[TenantContacts] (
    [Id] int NOT NULL IDENTITY,
    [TenantId] int NOT NULL,
    [Name] NVARCHAR(150) NOT NULL,
    [Email] NVARCHAR(255) NOT NULL,
    [Phone] NVARCHAR(30) NULL,
    [IsPrimary] BIT NOT NULL DEFAULT CAST(0 AS BIT),
    [IsActive] BIT NOT NULL DEFAULT CAST(1 AS BIT),
    [IsDeleted] BIT NOT NULL DEFAULT CAST(0 AS BIT),
    [CreatedBy] NVARCHAR(50) NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT (SYSDATETIME()),
    [ModifiedBy] NVARCHAR(50) NULL,
    [ModifiedAt] DATETIME2 NULL,
    CONSTRAINT [PK_TenantContacts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TenantContacts_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenants] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [dbo].[TenantFiscalData] (
    [Id] int NOT NULL IDENTITY,
    [TenantId] int NOT NULL,
    [NIF] CHAR(9) NOT NULL,
    [VATNumber] NVARCHAR(20) NOT NULL,
    [CAE] NVARCHAR(10) NULL,
    [FiscalCountry] CHAR(2) NOT NULL DEFAULT 'PT',
    [IsVATRegistered] BIT NOT NULL DEFAULT CAST(1 AS BIT),
    [IsActive] BIT NOT NULL DEFAULT CAST(1 AS BIT),
    [IsDeleted] BIT NOT NULL DEFAULT CAST(0 AS BIT),
    [CreatedBy] NVARCHAR(50) NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT (SYSDATETIME()),
    [ModifiedBy] NVARCHAR(50) NULL,
    [ModifiedAt] DATETIME2 NULL,
    CONSTRAINT [PK_TenantFiscalData] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TenantFiscalData_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenants] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [dbo].[Users] (
    [Id] int NOT NULL IDENTITY,
    [TenantId] int NOT NULL,
    [Email] NVARCHAR(255) NOT NULL,
    [PasswordHash] VARBINARY(64) NOT NULL,
    [FullName] NVARCHAR(150) NOT NULL,
    [IsActive] BIT NOT NULL DEFAULT CAST(1 AS BIT),
    [IsDeleted] BIT NOT NULL DEFAULT CAST(0 AS BIT),
    [CreatedBy] NVARCHAR(50) NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT (SYSDATETIME()),
    [ModifiedBy] NVARCHAR(50) NULL,
    [ModifiedAt] DATETIME2 NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Users_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenants] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [dbo].[Vehicles] (
    [Id] int NOT NULL IDENTITY,
    [TenantId] int NOT NULL,
    [Plate] NVARCHAR(20) NOT NULL,
    [Model] NVARCHAR(100) NULL,
    [IsActive] BIT NOT NULL DEFAULT CAST(1 AS BIT),
    [IsDeleted] BIT NOT NULL DEFAULT CAST(0 AS BIT),
    [CreatedBy] NVARCHAR(50) NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT (SYSDATETIME()),
    [ModifiedBy] NVARCHAR(50) NULL,
    [ModifiedAt] DATETIME2 NULL,
    CONSTRAINT [PK_Vehicles] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Vehicles_Tenants] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenants] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [dbo].[ClientAddresses] (
    [Id] int NOT NULL IDENTITY,
    [TenantId] int NOT NULL,
    [ClientId] int NOT NULL,
    [Street] NVARCHAR(200) NOT NULL,
    [City] NVARCHAR(100) NOT NULL,
    [PostalCode] NVARCHAR(20) NOT NULL,
    [District] NVARCHAR(100) NULL,
    [CountryCode] CHAR(2) NOT NULL DEFAULT 'PT',
    [IsPrimary] BIT NOT NULL DEFAULT CAST(0 AS BIT),
    [IsActive] BIT NOT NULL DEFAULT CAST(1 AS BIT),
    [IsDeleted] BIT NOT NULL DEFAULT CAST(0 AS BIT),
    [CreatedBy] NVARCHAR(50) NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT (SYSDATETIME()),
    [ModifiedBy] NVARCHAR(50) NULL,
    [ModifiedAt] DATETIME2 NULL,
    CONSTRAINT [PK_ClientAddresses] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ClientAddresses_Client] FOREIGN KEY ([ClientId]) REFERENCES [dbo].[Clients] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_ClientAddresses_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenants] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [dbo].[ClientContacts] (
    [Id] int NOT NULL IDENTITY,
    [TenantId] int NOT NULL,
    [ClientId] int NOT NULL,
    [Name] NVARCHAR(150) NOT NULL,
    [Email] NVARCHAR(255) NOT NULL,
    [Phone] NVARCHAR(30) NULL,
    [IsPrimary] BIT NOT NULL DEFAULT CAST(0 AS BIT),
    [IsActive] BIT NOT NULL DEFAULT CAST(1 AS BIT),
    [IsDeleted] BIT NOT NULL DEFAULT CAST(0 AS BIT),
    [CreatedBy] NVARCHAR(50) NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT (SYSDATETIME()),
    [ModifiedBy] NVARCHAR(50) NULL,
    [ModifiedAt] DATETIME2 NULL,
    CONSTRAINT [PK_ClientContacts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ClientContacts_Client] FOREIGN KEY ([ClientId]) REFERENCES [dbo].[Clients] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_ClientContacts_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenants] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [dbo].[ClientFiscalData] (
    [Id] int NOT NULL IDENTITY,
    [TenantId] int NOT NULL,
    [ClientId] int NOT NULL,
    [NIF] CHAR(9) NOT NULL,
    [VATNumber] NVARCHAR(20) NOT NULL,
    [CAE] NVARCHAR(10) NULL,
    [FiscalCountry] CHAR(2) NOT NULL DEFAULT 'PT',
    [IsVATRegistered] BIT NOT NULL DEFAULT CAST(1 AS BIT),
    [IsActive] BIT NOT NULL DEFAULT CAST(1 AS BIT),
    [IsDeleted] BIT NOT NULL DEFAULT CAST(0 AS BIT),
    [CreatedBy] NVARCHAR(50) NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT (SYSDATETIME()),
    [ModifiedBy] NVARCHAR(50) NULL,
    [ModifiedAt] DATETIME2 NULL,
    CONSTRAINT [PK_ClientFiscalData] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ClientFiscalData_Client] FOREIGN KEY ([ClientId]) REFERENCES [dbo].[Clients] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_ClientFiscalData_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenants] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [dbo].[RolePermissions] (
    [Id] int NOT NULL IDENTITY,
    [TenantId] int NOT NULL,
    [RoleId] int NOT NULL,
    [ResourceId] int NOT NULL,
    [ActionId] int NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [ModifiedBy] nvarchar(max) NULL,
    [ModifiedAt] datetime2 NULL,
    CONSTRAINT [PK_RolePermissions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_RolePermissions_Action] FOREIGN KEY ([ActionId]) REFERENCES [dbo].[Actions] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_RolePermissions_Resource] FOREIGN KEY ([ResourceId]) REFERENCES [dbo].[Resources] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_RolePermissions_Role] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_RolePermissions_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenants] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [dbo].[TeamMemberAddresses] (
    [Id] int NOT NULL IDENTITY,
    [TenantId] int NOT NULL,
    [TeamMemberId] int NOT NULL,
    [Street] NVARCHAR(200) NOT NULL,
    [City] NVARCHAR(100) NOT NULL,
    [PostalCode] NVARCHAR(20) NOT NULL,
    [District] NVARCHAR(100) NULL,
    [CountryCode] CHAR(2) NOT NULL DEFAULT 'PT',
    [IsPrimary] BIT NOT NULL DEFAULT CAST(0 AS BIT),
    [IsActive] BIT NOT NULL DEFAULT CAST(1 AS BIT),
    [IsDeleted] BIT NOT NULL DEFAULT CAST(0 AS BIT),
    [CreatedBy] NVARCHAR(50) NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT (SYSDATETIME()),
    [ModifiedBy] NVARCHAR(50) NULL,
    [ModifiedAt] DATETIME2 NULL,
    CONSTRAINT [PK_TeamMemberAddresses] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TeamMemberAddresses_TeamMember] FOREIGN KEY ([TeamMemberId]) REFERENCES [dbo].[TeamMembers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_TeamMemberAddresses_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenants] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [dbo].[TeamMemberContacts] (
    [Id] int NOT NULL IDENTITY,
    [TenantId] int NOT NULL,
    [TeamMemberId] int NOT NULL,
    [Name] NVARCHAR(150) NOT NULL,
    [Email] NVARCHAR(255) NOT NULL,
    [Phone] NVARCHAR(30) NULL,
    [IsPrimary] BIT NOT NULL DEFAULT CAST(0 AS BIT),
    [IsActive] BIT NOT NULL DEFAULT CAST(1 AS BIT),
    [IsDeleted] BIT NOT NULL DEFAULT CAST(0 AS BIT),
    [CreatedBy] NVARCHAR(50) NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT (SYSDATETIME()),
    [ModifiedBy] NVARCHAR(50) NULL,
    [ModifiedAt] DATETIME2 NULL,
    CONSTRAINT [PK_TeamMemberContacts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TeamMemberContacts_TeamMember] FOREIGN KEY ([TeamMemberId]) REFERENCES [dbo].[TeamMembers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_TeamMemberContacts_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenants] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [dbo].[UserRoles] (
    [Id] int NOT NULL IDENTITY,
    [TenantId] int NOT NULL,
    [UserId] int NOT NULL,
    [RoleId] int NOT NULL,
    [RoleId1] int NULL,
    [CreatedBy] nvarchar(max) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [ModifiedBy] nvarchar(max) NULL,
    [ModifiedAt] datetime2 NULL,
    CONSTRAINT [PK_UserRoles] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UserRoles_Role] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_UserRoles_Roles_RoleId1] FOREIGN KEY ([RoleId1]) REFERENCES [dbo].[Roles] ([Id]),
    CONSTRAINT [FK_UserRoles_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenants] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_UserRoles_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [dbo].[Interventions] (
    [Id] int NOT NULL IDENTITY,
    [TenantId] int NOT NULL,
    [ClientId] int NOT NULL,
    [TeamMemberId] int NOT NULL,
    [VehicleId] int NOT NULL,
    [Title] NVARCHAR(200) NOT NULL,
    [Description] NVARCHAR(2000) NOT NULL,
    [StartDateTime] DATETIME2 NOT NULL,
    [EndDateTime] DATETIME2 NULL,
    [EstimatedValue] DECIMAL(10,2) NOT NULL,
    [RealValue] DECIMAL(10,2) NULL,
    [Status] TINYINT NOT NULL,
    [IsActive] BIT NOT NULL DEFAULT CAST(1 AS BIT),
    [IsDeleted] BIT NOT NULL DEFAULT CAST(0 AS BIT),
    [CreatedBy] NVARCHAR(50) NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT (SYSDATETIME()),
    [ModifiedBy] NVARCHAR(50) NULL,
    [ModifiedAt] DATETIME2 NULL,
    CONSTRAINT [PK_Interventions] PRIMARY KEY ([Id]),
    CONSTRAINT [CK_EstimatedValue] CHECK ([EstimatedValue] >= 0),
    CONSTRAINT [CK_Interventions_EndDateTime] CHECK ([EndDateTime] IS NULL OR [EndDateTime] >= [StartDateTime]),
    CONSTRAINT [FK_Interventions_Clients] FOREIGN KEY ([ClientId]) REFERENCES [dbo].[Clients] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Interventions_TeamMembers] FOREIGN KEY ([TeamMemberId]) REFERENCES [dbo].[TeamMembers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Interventions_Tenants] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenants] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Interventions_Vehicles_VehicleId] FOREIGN KEY ([VehicleId]) REFERENCES [dbo].[Vehicles] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [dbo].[InterventionAddresses] (
    [Id] int NOT NULL IDENTITY,
    [TenantId] int NOT NULL,
    [InterventionId] int NOT NULL,
    [Street] NVARCHAR(200) NOT NULL,
    [City] NVARCHAR(100) NOT NULL,
    [PostalCode] NVARCHAR(20) NOT NULL,
    [District] NVARCHAR(100) NULL,
    [CountryCode] CHAR(2) NOT NULL DEFAULT 'PT',
    [IsPrimary] BIT NOT NULL DEFAULT CAST(0 AS BIT),
    [IsActive] BIT NOT NULL DEFAULT CAST(1 AS BIT),
    [IsDeleted] BIT NOT NULL DEFAULT CAST(0 AS BIT),
    [CreatedBy] NVARCHAR(50) NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT (SYSDATETIME()),
    [ModifiedBy] NVARCHAR(50) NULL,
    [ModifiedAt] DATETIME2 NULL,
    CONSTRAINT [PK_InterventionAddresses] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_InterventionAddresses_Intervention] FOREIGN KEY ([InterventionId]) REFERENCES [dbo].[Interventions] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_InterventionAddresses_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenants] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [dbo].[InterventionContacts] (
    [Id] int NOT NULL IDENTITY,
    [TenantId] int NOT NULL,
    [InterventionId] int NOT NULL,
    [Name] NVARCHAR(150) NOT NULL,
    [Email] NVARCHAR(255) NOT NULL,
    [Phone] NVARCHAR(30) NULL,
    [IsPrimary] BIT NOT NULL DEFAULT CAST(0 AS BIT),
    [IsActive] BIT NOT NULL DEFAULT CAST(1 AS BIT),
    [IsDeleted] BIT NOT NULL DEFAULT CAST(0 AS BIT),
    [CreatedBy] NVARCHAR(50) NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT (SYSDATETIME()),
    [ModifiedBy] NVARCHAR(50) NULL,
    [ModifiedAt] DATETIME2 NULL,
    CONSTRAINT [PK_InterventionContacts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_InterventionContacts_Intervention] FOREIGN KEY ([InterventionId]) REFERENCES [dbo].[Interventions] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_InterventionContacts_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenants] ([Id]) ON DELETE NO ACTION
);
GO

CREATE UNIQUE INDEX [UQ_Actions_Name] ON [dbo].[Actions] ([Name]);
GO

CREATE UNIQUE INDEX [IX_ClientAddresses_ClientId] ON [dbo].[ClientAddresses] ([ClientId]) INCLUDE ([TenantId]) WHERE [IsDeleted] = 0;
GO

CREATE INDEX [IX_ClientAddresses_TenantId] ON [dbo].[ClientAddresses] ([TenantId]);
GO

CREATE INDEX [IX_ClientContacts_ClientId] ON [dbo].[ClientContacts] ([ClientId]) INCLUDE ([TenantId]) WHERE [IsDeleted] = 0;
GO

CREATE INDEX [IX_ClientContacts_TenantId] ON [dbo].[ClientContacts] ([TenantId]);
GO

CREATE INDEX [IX_ClientFiscalData_ClientId] ON [dbo].[ClientFiscalData] ([ClientId]) INCLUDE ([TenantId]) WHERE [IsDeleted] = 0;
GO

CREATE INDEX [IX_ClientFiscalData_TenantId] ON [dbo].[ClientFiscalData] ([TenantId]);
GO

CREATE UNIQUE INDEX [UQ_ClientFiscalData_NIF] ON [dbo].[ClientFiscalData] ([NIF]);
GO

CREATE UNIQUE INDEX [IX_Clients_Tenant_Active] ON [dbo].[Clients] ([TenantId], [Name]) INCLUDE ([Email], [Phone]) WHERE [IsDeleted] = 0;
GO

CREATE INDEX [IX_Equipments_TenantId] ON [dbo].[Equipments] ([TenantId]);
GO

CREATE INDEX [IX_InterventionAddresses_InterventionId] ON [dbo].[InterventionAddresses] ([InterventionId]) INCLUDE ([TenantId]) WHERE [IsDeleted] = 0;
GO

CREATE INDEX [IX_InterventionAddresses_TenantId] ON [dbo].[InterventionAddresses] ([TenantId]);
GO

CREATE INDEX [IX_InterventionContacts_InterventionId] ON [dbo].[InterventionContacts] ([InterventionId]) INCLUDE ([TenantId]) WHERE [IsDeleted] = 0;
GO

CREATE INDEX [IX_InterventionContacts_TenantId] ON [dbo].[InterventionContacts] ([TenantId]);
GO

CREATE INDEX [IX_Interventions_ClientId] ON [dbo].[Interventions] ([ClientId]);
GO

CREATE INDEX [IX_Interventions_TeamMemberId] ON [dbo].[Interventions] ([TeamMemberId]);
GO

CREATE INDEX [IX_Interventions_Tenant_Date] ON [dbo].[Interventions] ([TenantId], [StartDateTime]) INCLUDE ([ClientId], [TeamMemberId], [Status]) WHERE [IsDeleted] = 0;
GO

CREATE INDEX [IX_Interventions_VehicleId] ON [dbo].[Interventions] ([VehicleId]);
GO

CREATE UNIQUE INDEX [UQ_Resources_Name] ON [dbo].[Resources] ([Name]);
GO

CREATE INDEX [IX_RolePermissions_ActionId] ON [dbo].[RolePermissions] ([ActionId]) INCLUDE ([TenantId], [RoleId], [ResourceId]);
GO

CREATE INDEX [IX_RolePermissions_ResourceId] ON [dbo].[RolePermissions] ([ResourceId]) INCLUDE ([TenantId], [RoleId], [ActionId]);
GO

CREATE INDEX [IX_RolePermissions_RoleId] ON [dbo].[RolePermissions] ([RoleId]) INCLUDE ([TenantId], [ResourceId], [ActionId]);
GO

CREATE UNIQUE INDEX [UQ_RolePermissions] ON [dbo].[RolePermissions] ([TenantId], [RoleId], [ResourceId], [ActionId]);
GO

CREATE UNIQUE INDEX [UQ_Roles_Tenant_Name] ON [dbo].[Roles] ([TenantId], [Name]);
GO

CREATE INDEX [IX_TeamMemberAddresses_TeamMemberId] ON [dbo].[TeamMemberAddresses] ([TeamMemberId]) INCLUDE ([TenantId]) WHERE [IsDeleted] = 0;
GO

CREATE INDEX [IX_TeamMemberAddresses_TenantId] ON [dbo].[TeamMemberAddresses] ([TenantId]);
GO

CREATE INDEX [IX_TeamMemberContacts_TeamMemberId] ON [dbo].[TeamMemberContacts] ([TeamMemberId]) INCLUDE ([TenantId]) WHERE [IsDeleted] = 0;
GO

CREATE INDEX [IX_TeamMemberContacts_TenantId] ON [dbo].[TeamMemberContacts] ([TenantId]);
GO

CREATE INDEX [IX_TeamMembers_TenantId] ON [dbo].[TeamMembers] ([TenantId]);
GO

CREATE INDEX [IX_TenantAddresses_TenantId] ON [dbo].[TenantAddresses] ([TenantId]);
GO

CREATE INDEX [IX_TenantContacts_TenantId] ON [dbo].[TenantContacts] ([TenantId]) WHERE [IsDeleted] = 0;
GO

CREATE INDEX [IX_TenantFiscalData_TenantId] ON [dbo].[TenantFiscalData] ([TenantId]);
GO

CREATE UNIQUE INDEX [UQ_TenantFiscalData_NIF] ON [dbo].[TenantFiscalData] ([NIF]);
GO

CREATE INDEX [IX_UserRoles_RoleId] ON [dbo].[UserRoles] ([RoleId]) INCLUDE ([TenantId], [UserId]);
GO

CREATE INDEX [IX_UserRoles_RoleId1] ON [dbo].[UserRoles] ([RoleId1]);
GO

CREATE INDEX [IX_UserRoles_UserId] ON [dbo].[UserRoles] ([UserId]) INCLUDE ([TenantId], [RoleId]);
GO

CREATE UNIQUE INDEX [UQ_UserRoles] ON [dbo].[UserRoles] ([TenantId], [UserId], [RoleId]);
GO

CREATE UNIQUE INDEX [IX_Users_Login] ON [dbo].[Users] ([TenantId], [Email]) INCLUDE ([Id], [IsActive]) WHERE [IsDeleted] = 0;
GO

CREATE UNIQUE INDEX [UQ_Vehicles_Tenant_Plate] ON [dbo].[Vehicles] ([TenantId], [Plate]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260114165931_InitialCreate', N'8.0.11');
GO

COMMIT;
GO

