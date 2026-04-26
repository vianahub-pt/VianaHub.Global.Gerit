-- VISITS (mais dependente)
TRUNCATE TABLE dbo.VisitAttachments;
TRUNCATE TABLE dbo.VisitTeamEquipment;
TRUNCATE TABLE dbo.VisitTeamVehicle;
TRUNCATE TABLE dbo.VisitTeamEmployee;
TRUNCATE TABLE dbo.VisitAddresses;
TRUNCATE TABLE dbo.VisitContacts;
TRUNCATE TABLE dbo.VisitTeam;
TRUNCATE TABLE dbo.Visits;

-- RESOURCES
TRUNCATE TABLE dbo.Vehicles;
TRUNCATE TABLE dbo.Equipments;
TRUNCATE TABLE dbo.EquipmentTypes;

-- EMPLOYEES
TRUNCATE TABLE dbo.EmployeeTeam;
TRUNCATE TABLE dbo.EmployeeAddresses;
TRUNCATE TABLE dbo.EmployeeContacts;
TRUNCATE TABLE dbo.Employees;
TRUNCATE TABLE dbo.Teams;

-- CLIENTS (ordem crítica)
TRUNCATE TABLE dbo.ClientConsents;
TRUNCATE TABLE dbo.ClientHierarchy;
TRUNCATE TABLE dbo.ClientFiscalData;
TRUNCATE TABLE dbo.ClientContacts;
TRUNCATE TABLE dbo.ClientAddresses;
TRUNCATE TABLE dbo.ClientCompanies;
TRUNCATE TABLE dbo.ClientIndividuals;
TRUNCATE TABLE dbo.Clients;

-- AUTH / SECURITY
TRUNCATE TABLE dbo.RefreshTokens;
TRUNCATE TABLE dbo.UserRoles;
TRUNCATE TABLE dbo.RolePermissions;
TRUNCATE TABLE dbo.UserPreferences;
TRUNCATE TABLE dbo.Users;
TRUNCATE TABLE dbo.Roles;
TRUNCATE TABLE dbo.Actions;
TRUNCATE TABLE dbo.Resources;

-- TENANT
TRUNCATE TABLE dbo.TenantFiscalData;
TRUNCATE TABLE dbo.TenantAddresses;
TRUNCATE TABLE dbo.TenantContacts;
TRUNCATE TABLE dbo.Subscriptions;
TRUNCATE TABLE dbo.Tenants;

-- CONFIG / MASTER DATA
TRUNCATE TABLE dbo.Functions;
TRUNCATE TABLE dbo.JobDefinitions;
TRUNCATE TABLE dbo.JwtKeys;
TRUNCATE TABLE dbo.Status;
TRUNCATE TABLE dbo.PlanFileRules;
TRUNCATE TABLE dbo.Plans;
TRUNCATE TABLE dbo.StatusTypes;
TRUNCATE TABLE dbo.OriginTypes;
TRUNCATE TABLE dbo.ConsentTypes;
TRUNCATE TABLE dbo.FileTypes;
TRUNCATE TABLE dbo.AddressTypes;
TRUNCATE TABLE dbo.AttachmentCategories;