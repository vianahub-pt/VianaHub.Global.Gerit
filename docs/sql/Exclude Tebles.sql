/* =========================================================
   1. DESATIVAR E REMOVER ROW LEVEL SECURITY
   ========================================================= */

IF EXISTS (SELECT 1 FROM sys.security_policies WHERE name = 'TenantSecurityPolicy')
BEGIN
    DROP SECURITY POLICY dbo.TenantSecurityPolicy;
END
GO

IF OBJECT_ID('dbo.fn_TenantAccessPredicate', 'IF') IS NOT NULL
BEGIN
    DROP FUNCTION dbo.fn_TenantAccessPredicate;
END
GO

/* =========================================================
   2. REMOVER TABELAS (ORDEM INVERSA DE DEPENDÊNCIA)
   ========================================================= */

-- N:N e dependências finais
DROP TABLE IF EXISTS dbo.InterventionTeamEquipments;
DROP TABLE IF EXISTS dbo.InterventionTeamVehicles;
DROP TABLE IF EXISTS dbo.InterventionTeams;
DROP TABLE IF EXISTS dbo.TeamMembersTeams;

-- Interventions
DROP TABLE IF EXISTS dbo.InterventionAddresses;
DROP TABLE IF EXISTS dbo.InterventionContacts;
DROP TABLE IF EXISTS dbo.Interventions;

-- Team Members
DROP TABLE IF EXISTS dbo.TeamMemberAddresses;
DROP TABLE IF EXISTS dbo.TeamMemberContacts;
DROP TABLE IF EXISTS dbo.TeamMembers;

DROP TABLE IF EXISTS dbo.Teams;

-- Clients
DROP TABLE IF EXISTS dbo.ClientAddresses;
DROP TABLE IF EXISTS dbo.ClientContacts;
DROP TABLE IF EXISTS dbo.ClientFiscalData;
DROP TABLE IF EXISTS dbo.Clients;

-- RBAC
DROP TABLE IF EXISTS dbo.UserRoles;
DROP TABLE IF EXISTS dbo.RolePermissions;
DROP TABLE IF EXISTS dbo.Actions;
DROP TABLE IF EXISTS dbo.Resources;
DROP TABLE IF EXISTS dbo.Roles;

-- Security / Auth
DROP TABLE IF EXISTS dbo.RefreshTokens;
DROP TABLE IF EXISTS dbo.JwtKeys;
DROP TABLE IF EXISTS dbo.Users;

-- Tenant related
DROP TABLE IF EXISTS dbo.Subscriptions;
DROP TABLE IF EXISTS dbo.TenantFiscalData;
DROP TABLE IF EXISTS dbo.TenantAddresses;
DROP TABLE IF EXISTS dbo.TenantContacts;
DROP TABLE IF EXISTS dbo.AddressTypes;

-- Supporting / Catalog
DROP TABLE IF EXISTS dbo.Equipments;
DROP TABLE IF EXISTS dbo.Vehicles;
DROP TABLE IF EXISTS dbo.Status;
DROP TABLE IF EXISTS dbo.StatusTypes;       
DROP TABLE IF EXISTS dbo.EquipmentTypes;
DROP TABLE IF EXISTS dbo.Functions;

-- Jobs
DROP TABLE IF EXISTS dbo.JobDefinitions;

-- Core
DROP TABLE IF EXISTS dbo.Plans;
DROP TABLE IF EXISTS dbo.Tenants;

GO
