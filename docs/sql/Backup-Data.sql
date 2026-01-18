/* =========================
   BACKUP DOS DADOS EXISTENTES
   Execute este script ANTES de recriar o banco
   ========================= */

-- Backup Plans
SELECT * INTO #BackupPlans FROM dbo.Plans;
PRINT 'Plans backup: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Backup Tenants
SELECT * INTO #BackupTenants FROM dbo.Tenants;
PRINT 'Tenants backup: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Backup TenantContacts
SELECT * INTO #BackupTenantContacts FROM dbo.TenantContacts;
PRINT 'TenantContacts backup: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Backup TenantAddresses
SELECT * INTO #BackupTenantAddresses FROM dbo.TenantAddresses;
PRINT 'TenantAddresses backup: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Backup TenantFiscalData
SELECT * INTO #BackupTenantFiscalData FROM dbo.TenantFiscalData;
PRINT 'TenantFiscalData backup: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Backup Subscriptions
SELECT * INTO #BackupSubscriptions FROM dbo.Subscriptions;
PRINT 'Subscriptions backup: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Backup Users
SELECT * INTO #BackupUsers FROM dbo.Users;
PRINT 'Users backup: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Backup Roles
SELECT * INTO #BackupRoles FROM dbo.Roles;
PRINT 'Roles backup: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Backup Resources
SELECT * INTO #BackupResources FROM dbo.Resources;
PRINT 'Resources backup: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Backup Actions
SELECT * INTO #BackupActions FROM dbo.Actions;
PRINT 'Actions backup: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Backup RolePermissions
SELECT * INTO #BackupRolePermissions FROM dbo.RolePermissions;
PRINT 'RolePermissions backup: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Backup UserRoles
SELECT * INTO #BackupUserRoles FROM dbo.UserRoles;
PRINT 'UserRoles backup: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Backup JwtKeys
SELECT * INTO #BackupJwtKeys FROM dbo.JwtKeys;
PRINT 'JwtKeys backup: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Backup Clients
SELECT * INTO #BackupClients FROM dbo.Clients;
PRINT 'Clients backup: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Backup ClientContacts
SELECT * INTO #BackupClientContacts FROM dbo.ClientContacts;
PRINT 'ClientContacts backup: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Backup ClientAddresses
SELECT * INTO #BackupClientAddresses FROM dbo.ClientAddresses;
PRINT 'ClientAddresses backup: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Backup ClientFiscalData
SELECT * INTO #BackupClientFiscalData FROM dbo.ClientFiscalData;
PRINT 'ClientFiscalData backup: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Backup TeamMembers
SELECT * INTO #BackupTeamMembers FROM dbo.TeamMembers;
PRINT 'TeamMembers backup: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Backup TeamMemberContacts
SELECT * INTO #BackupTeamMemberContacts FROM dbo.TeamMemberContacts;
PRINT 'TeamMemberContacts backup: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Backup TeamMemberAddresses
SELECT * INTO #BackupTeamMemberAddresses FROM dbo.TeamMemberAddresses;
PRINT 'TeamMemberAddresses backup: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Backup Equipments
SELECT * INTO #BackupEquipments FROM dbo.Equipments;
PRINT 'Equipments backup: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Backup Vehicles
SELECT * INTO #BackupVehicles FROM dbo.Vehicles;
PRINT 'Vehicles backup: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Backup Interventions
SELECT * INTO #BackupInterventions FROM dbo.Interventions;
PRINT 'Interventions backup: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Backup InterventionContacts
SELECT * INTO #BackupInterventionContacts FROM dbo.InterventionContacts;
PRINT 'InterventionContacts backup: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

-- Backup InterventionAddresses
SELECT * INTO #BackupInterventionAddresses FROM dbo.InterventionAddresses;
PRINT 'InterventionAddresses backup: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros';

PRINT '=========================';
PRINT 'BACKUP COMPLETO!';
PRINT 'Os dados estão salvos em tabelas temporárias (#Backup*)';
PRINT '=========================';
GO
