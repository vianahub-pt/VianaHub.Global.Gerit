/* =========================
   RESTORE DOS DADOS APÓS RECRIAR AS TABELAS
   Execute este script DEPOIS de aplicar a nova migração
   ========================= */

-- Desabilita RLS temporariamente para restaurar dados
EXEC sp_executesql N'EXEC sp_set_session_context @key = N''IsSuperAdmin'', @value = 1;';
GO

-- Restore Plans
SET IDENTITY_INSERT dbo.Plans ON;
INSERT INTO dbo.Plans (Id, Name, Description, PricePerHour, PricePerDay, PricePerMonth, PricePerYear, Currency, MaxUsers, MaxPhotosPerInterventions, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt)
SELECT Id, Name, Description, PricePerHour, PricePerDay, PricePerMonth, PricePerYear, Currency, MaxUsers, MaxPhotosPerInterventions, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt 
FROM #BackupPlans;
SET IDENTITY_INSERT dbo.Plans OFF;
PRINT 'Plans restaurado: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Restore Tenants
SET IDENTITY_INSERT dbo.Tenants ON;
INSERT INTO dbo.Tenants (Id, LegalName, TradeName, Consent, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt)
SELECT Id, LegalName, TradeName, Consent, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt 
FROM #BackupTenants;
SET IDENTITY_INSERT dbo.Tenants OFF;
PRINT 'Tenants restaurado: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Restore TenantContacts
SET IDENTITY_INSERT dbo.TenantContacts ON;
INSERT INTO dbo.TenantContacts (Id, TenantId, Name, Email, Phone, IsPrimary, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt)
SELECT Id, TenantId, Name, Email, Phone, IsPrimary, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt 
FROM #BackupTenantContacts;
SET IDENTITY_INSERT dbo.TenantContacts OFF;
PRINT 'TenantContacts restaurado: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Restore TenantAddresses
SET IDENTITY_INSERT dbo.TenantAddresses ON;
INSERT INTO dbo.TenantAddresses (Id, TenantId, Street, City, PostalCode, District, CountryCode, IsPrimary, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt)
SELECT Id, TenantId, Street, City, PostalCode, District, CountryCode, IsPrimary, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt 
FROM #BackupTenantAddresses;
SET IDENTITY_INSERT dbo.TenantAddresses OFF;
PRINT 'TenantAddresses restaurado: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Restore TenantFiscalData
SET IDENTITY_INSERT dbo.TenantFiscalData ON;
INSERT INTO dbo.TenantFiscalData (Id, TenantId, NIF, VATNumber, CAE, FiscalCountry, IsVATRegistered, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt)
SELECT Id, TenantId, NIF, VATNumber, CAE, FiscalCountry, IsVATRegistered, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt 
FROM #BackupTenantFiscalData;
SET IDENTITY_INSERT dbo.TenantFiscalData OFF;
PRINT 'TenantFiscalData restaurado: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Restore Subscriptions
SET IDENTITY_INSERT dbo.Subscriptions ON;
INSERT INTO dbo.Subscriptions (Id, TenantId, PlanId, StripeId, CurrentPeriodStart, CurrentPeriodEnd, TrialStart, TrialEnd, CancelAtPeriodEnd, CanceledAt, CancellationReason, StripeCustomerId, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt)
SELECT Id, TenantId, PlanId, StripeId, CurrentPeriodStart, CurrentPeriodEnd, TrialStart, TrialEnd, CancelAtPeriodEnd, CanceledAt, CancellationReason, StripeCustomerId, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt 
FROM #BackupSubscriptions;
SET IDENTITY_INSERT dbo.Subscriptions OFF;
PRINT 'Subscriptions restaurado: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Restore Users
SET IDENTITY_INSERT dbo.Users ON;
INSERT INTO dbo.Users (Id, TenantId, Name, Email, NormalizedEmail, EmailConfirmed, PhoneNumber, PhoneNumberConfirmed, LastAccessAt, PasswordHash, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt)
SELECT Id, TenantId, Name, Email, NormalizedEmail, EmailConfirmed, PhoneNumber, PhoneNumberConfirmed, LastAccessAt, PasswordHash, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt 
FROM #BackupUsers;
SET IDENTITY_INSERT dbo.Users OFF;
PRINT 'Users restaurado: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Restore Roles
SET IDENTITY_INSERT dbo.Roles ON;
INSERT INTO dbo.Roles (Id, TenantId, Name, Description, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt)
SELECT Id, TenantId, Name, Description, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt 
FROM #BackupRoles;
SET IDENTITY_INSERT dbo.Roles OFF;
PRINT 'Roles restaurado: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Restore Resources
SET IDENTITY_INSERT dbo.Resources ON;
INSERT INTO dbo.Resources (Id, Name, Description, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt)
SELECT Id, Name, Description, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt 
FROM #BackupResources;
SET IDENTITY_INSERT dbo.Resources OFF;
PRINT 'Resources restaurado: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Restore Actions
SET IDENTITY_INSERT dbo.Actions ON;
INSERT INTO dbo.Actions (Id, Name, Description, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt)
SELECT Id, Name, Description, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt 
FROM #BackupActions;
SET IDENTITY_INSERT dbo.Actions OFF;
PRINT 'Actions restaurado: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Restore RolePermissions
SET IDENTITY_INSERT dbo.RolePermissions ON;
INSERT INTO dbo.RolePermissions (Id, TenantId, RoleId, ResourceId, ActionId)
SELECT Id, TenantId, RoleId, ResourceId, ActionId 
FROM #BackupRolePermissions;
SET IDENTITY_INSERT dbo.RolePermissions OFF;
PRINT 'RolePermissions restaurado: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Restore UserRoles
SET IDENTITY_INSERT dbo.UserRoles ON;
INSERT INTO dbo.UserRoles (Id, TenantId, UserId, RoleId)
SELECT Id, TenantId, UserId, RoleId 
FROM #BackupUserRoles;
SET IDENTITY_INSERT dbo.UserRoles OFF;
PRINT 'UserRoles restaurado: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Restore JwtKeys
SET IDENTITY_INSERT dbo.JwtKeys ON;
INSERT INTO dbo.JwtKeys (Id, TenantId, ApplicationId, KeyId, Code, PublicKey, PrivateKeyEncrypted, Algorithm, KeySize, KeyType, RevokedReason, UsageCount, ActivatedAt, ExpiresAt, LastUsedAt, NextRotationAt, RevokedAt, LastValidatedAt, ValidationCount, RotationPolicyDays, OverlapPeriodDays, MaxTokenLifetimeMinutes, Status, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt)
SELECT Id, TenantId, ApplicationId, KeyId, Code, PublicKey, PrivateKeyEncrypted, Algorithm, KeySize, KeyType, RevokedReason, UsageCount, ActivatedAt, ExpiresAt, LastUsedAt, NextRotationAt, RevokedAt, LastValidatedAt, ValidationCount, RotationPolicyDays, OverlapPeriodDays, MaxTokenLifetimeMinutes, Status, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt 
FROM #BackupJwtKeys;
SET IDENTITY_INSERT dbo.JwtKeys OFF;
PRINT 'JwtKeys restaurado: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Restore Clients
SET IDENTITY_INSERT dbo.Clients ON;
INSERT INTO dbo.Clients (Id, TenantId, Name, Email, Phone, Consent, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt)
SELECT Id, TenantId, Name, Email, Phone, Consent, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt 
FROM #BackupClients;
SET IDENTITY_INSERT dbo.Clients OFF;
PRINT 'Clients restaurado: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Restore ClientContacts
SET IDENTITY_INSERT dbo.ClientContacts ON;
INSERT INTO dbo.ClientContacts (Id, TenantId, ClientId, Name, Email, Phone, IsPrimary, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt)
SELECT Id, TenantId, ClientId, Name, Email, Phone, IsPrimary, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt 
FROM #BackupClientContacts;
SET IDENTITY_INSERT dbo.ClientContacts OFF;
PRINT 'ClientContacts restaurado: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Restore ClientAddresses
SET IDENTITY_INSERT dbo.ClientAddresses ON;
INSERT INTO dbo.ClientAddresses (Id, TenantId, ClientId, Street, City, PostalCode, District, CountryCode, IsPrimary, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt)
SELECT Id, TenantId, ClientId, Street, City, PostalCode, District, CountryCode, IsPrimary, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt 
FROM #BackupClientAddresses;
SET IDENTITY_INSERT dbo.ClientAddresses OFF;
PRINT 'ClientAddresses restaurado: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Restore ClientFiscalData
SET IDENTITY_INSERT dbo.ClientFiscalData ON;
INSERT INTO dbo.ClientFiscalData (Id, TenantId, ClientId, NIF, VATNumber, CAE, FiscalCountry, IsVATRegistered, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt)
SELECT Id, TenantId, ClientId, NIF, VATNumber, CAE, FiscalCountry, IsVATRegistered, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt 
FROM #BackupClientFiscalData;
SET IDENTITY_INSERT dbo.ClientFiscalData OFF;
PRINT 'ClientFiscalData restaurado: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Restore TeamMembers
SET IDENTITY_INSERT dbo.TeamMembers ON;
INSERT INTO dbo.TeamMembers (Id, TenantId, Name, TaxNumber, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt)
SELECT Id, TenantId, Name, TaxNumber, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt 
FROM #BackupTeamMembers;
SET IDENTITY_INSERT dbo.TeamMembers OFF;
PRINT 'TeamMembers restaurado: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Restore TeamMemberContacts
SET IDENTITY_INSERT dbo.TeamMemberContacts ON;
INSERT INTO dbo.TeamMemberContacts (Id, TenantId, TeamMemberId, Name, Email, Phone, IsPrimary, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt)
SELECT Id, TenantId, TeamMemberId, Name, Email, Phone, IsPrimary, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt 
FROM #BackupTeamMemberContacts;
SET IDENTITY_INSERT dbo.TeamMemberContacts OFF;
PRINT 'TeamMemberContacts restaurado: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Restore TeamMemberAddresses
SET IDENTITY_INSERT dbo.TeamMemberAddresses ON;
INSERT INTO dbo.TeamMemberAddresses (Id, TenantId, TeamMemberId, Street, City, PostalCode, District, CountryCode, IsPrimary, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt)
SELECT Id, TenantId, TeamMemberId, Street, City, PostalCode, District, CountryCode, IsPrimary, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt 
FROM #BackupTeamMemberAddresses;
SET IDENTITY_INSERT dbo.TeamMemberAddresses OFF;
PRINT 'TeamMemberAddresses restaurado: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Restore Equipments
SET IDENTITY_INSERT dbo.Equipments ON;
INSERT INTO dbo.Equipments (Id, TenantId, Name, SerialNumber, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt)
SELECT Id, TenantId, Name, SerialNumber, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt 
FROM #BackupEquipments;
SET IDENTITY_INSERT dbo.Equipments OFF;
PRINT 'Equipments restaurado: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Restore Vehicles
SET IDENTITY_INSERT dbo.Vehicles ON;
INSERT INTO dbo.Vehicles (Id, TenantId, Plate, Model, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt)
SELECT Id, TenantId, Plate, Model, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt 
FROM #BackupVehicles;
SET IDENTITY_INSERT dbo.Vehicles OFF;
PRINT 'Vehicles restaurado: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Restore Interventions
SET IDENTITY_INSERT dbo.Interventions ON;
INSERT INTO dbo.Interventions (Id, TenantId, ClientId, TeamMemberId, VehicleId, Title, Description, StartDateTime, EndDateTime, EstimatedValue, RealValue, Status, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt)
SELECT Id, TenantId, ClientId, TeamMemberId, VehicleId, Title, Description, StartDateTime, EndDateTime, EstimatedValue, RealValue, Status, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt 
FROM #BackupInterventions;
SET IDENTITY_INSERT dbo.Interventions OFF;
PRINT 'Interventions restaurado: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Restore InterventionContacts
SET IDENTITY_INSERT dbo.InterventionContacts ON;
INSERT INTO dbo.InterventionContacts (Id, TenantId, InterventionId, Name, Email, Phone, IsPrimary, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt)
SELECT Id, TenantId, InterventionId, Name, Email, Phone, IsPrimary, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt 
FROM #BackupInterventionContacts;
SET IDENTITY_INSERT dbo.InterventionContacts OFF;
PRINT 'InterventionContacts restaurado: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Restore InterventionAddresses
SET IDENTITY_INSERT dbo.InterventionAddresses ON;
INSERT INTO dbo.InterventionAddresses (Id, TenantId, InterventionId, Street, City, PostalCode, District, CountryCode, IsPrimary, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt)
SELECT Id, TenantId, InterventionId, Street, City, PostalCode, District, CountryCode, IsPrimary, IsActive, IsDeleted, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt 
FROM #BackupInterventionAddresses;
SET IDENTITY_INSERT dbo.InterventionAddresses OFF;
PRINT 'InterventionAddresses restaurado: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Limpar tabelas temporárias
DROP TABLE #BackupPlans;
DROP TABLE #BackupTenants;
DROP TABLE #BackupTenantContacts;
DROP TABLE #BackupTenantAddresses;
DROP TABLE #BackupTenantFiscalData;
DROP TABLE #BackupSubscriptions;
DROP TABLE #BackupUsers;
DROP TABLE #BackupRoles;
DROP TABLE #BackupResources;
DROP TABLE #BackupActions;
DROP TABLE #BackupRolePermissions;
DROP TABLE #BackupUserRoles;
DROP TABLE #BackupJwtKeys;
DROP TABLE #BackupClients;
DROP TABLE #BackupClientContacts;
DROP TABLE #BackupClientAddresses;
DROP TABLE #BackupClientFiscalData;
DROP TABLE #BackupTeamMembers;
DROP TABLE #BackupTeamMemberContacts;
DROP TABLE #BackupTeamMemberAddresses;
DROP TABLE #BackupEquipments;
DROP TABLE #BackupVehicles;
DROP TABLE #BackupInterventions;
DROP TABLE #BackupInterventionContacts;
DROP TABLE #BackupInterventionAddresses;

PRINT '=========================';
PRINT 'RESTORE COMPLETO!';
PRINT 'Todos os dados foram restaurados';
PRINT '=========================';
GO
