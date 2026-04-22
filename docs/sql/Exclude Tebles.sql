/* =========================================================
   1. DESATIVAR E REMOVER ROW LEVEL SECURITY
   ========================================================= */

IF EXISTS (SELECT 1 FROM sys.security_policies WHERE name = 'TenantSecurityPolicy')
BEGIN
    DROP SECURITY POLICY dbo.TenantSecurityPolicy;
END
GO
DROP SECURITY POLICY IF EXISTS dbo.TenantSecurityPolicy;
GO
DROP FUNCTION IF EXISTS dbo.fn_TenantAccessPredicate;
GO
DROP TABLE IF EXISTS dbo.VisitAttachments;
DROP TABLE IF EXISTS dbo.AttachmentCategories;
DROP TABLE IF EXISTS dbo.VisitTeamEquipment;
DROP TABLE IF EXISTS dbo.VisitTeamVehicle;
DROP TABLE IF EXISTS dbo.VisitTeamEmployee;
DROP TABLE IF EXISTS dbo.VisitTeam;
DROP TABLE IF EXISTS dbo.VisitAddresses;
DROP TABLE IF EXISTS dbo.VisitContacts;
DROP TABLE IF EXISTS dbo.Visits;
DROP TABLE IF EXISTS dbo.Vehicles;
DROP TABLE IF EXISTS dbo.Equipments;
DROP TABLE IF EXISTS dbo.EquipmentTypes;
DROP TABLE IF EXISTS dbo.EmployeeTeam;
DROP TABLE IF EXISTS dbo.EmployeeAddresses;
DROP TABLE IF EXISTS dbo.EmployeeContacts;
DROP TABLE IF EXISTS dbo.Employees;
DROP TABLE IF EXISTS dbo.Teams;
DROP TABLE IF EXISTS dbo.ClientConsents;
DROP TABLE IF EXISTS dbo.ClientHierarchy;
DROP TABLE IF EXISTS dbo.ClientCompanyFiscalData;
DROP TABLE IF EXISTS dbo.ClientIndividualFiscalData;
DROP TABLE IF EXISTS dbo.ClientContacts;
DROP TABLE IF EXISTS dbo.ClientAddresses;
DROP TABLE IF EXISTS dbo.ClientCompanies;
DROP TABLE IF EXISTS dbo.ClientIndividuals;
DROP TABLE IF EXISTS dbo.Clients;
DROP TABLE IF EXISTS dbo.Functions;
DROP TABLE IF EXISTS dbo.JobDefinitions;
DROP TABLE IF EXISTS dbo.JwtKeys;
DROP TABLE IF EXISTS dbo.RefreshTokens;
DROP TABLE IF EXISTS dbo.UserRoles;
DROP TABLE IF EXISTS dbo.RolePermissions;
DROP TABLE IF EXISTS dbo.Actions;
DROP TABLE IF EXISTS dbo.Resources;
DROP TABLE IF EXISTS dbo.Roles;
DROP TABLE IF EXISTS dbo.UserPreferences;
DROP TABLE IF EXISTS dbo.Users;
DROP TABLE IF EXISTS dbo.Subscriptions;
DROP TABLE IF EXISTS dbo.TenantFiscalData;
DROP TABLE IF EXISTS dbo.TenantAddresses;
DROP TABLE IF EXISTS dbo.TenantContacts;
DROP TABLE IF EXISTS dbo.Status;
DROP TABLE IF EXISTS dbo.Tenants;
DROP TABLE IF EXISTS dbo.PlanFileRules;
DROP TABLE IF EXISTS dbo.Plans;
DROP TABLE IF EXISTS dbo.StatusTypes;
DROP TABLE IF EXISTS dbo.OriginTypes;
DROP TABLE IF EXISTS dbo.ConsentTypes;
DROP TABLE IF EXISTS dbo.FileTypes;
DROP TABLE IF EXISTS dbo.AddressTypes;



GO
