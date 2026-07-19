EXEC sp_set_session_context @key=N'IsSuperAdmin', @value=1;

--------------------------------------------
select * from dbo.AcquisitionSourceTypes;
select * from dbo.AcquisitionSourceTypeTranslations;
select * from dbo.AddressTypes;
select * from dbo.AddressTypeTranslations;
select * from dbo.DocumentTypes;
select * from dbo.DocumentTypeTranslations;
select * from dbo.FileTypes;
select * from dbo.FileTypeTranslations;
select * from dbo.PartyTypes;
select * from dbo.PartyTypeTranslations;
select * from dbo.StatusDefinitions;
select * from dbo.StatusDefinitionTranslations;
select * from dbo.StatusDomains;
select * from dbo.StatusDomainTranslations;
select * from dbo.SubscriptionPlans;
select * from dbo.SubscriptionPlanTranslations;

--------------------------------------------
select * from dbo.Actions;
select * from dbo.Resources;
select * from dbo.Roles;

--------------------------------------------
select * from dbo.Tenants;
select * from dbo.TenantAddresses;
select * from dbo.TenantContactPersons;
select * from dbo.TenantDocuments;
select * from dbo.TenantFiscalData;
select * from dbo.Subscriptions;

--------------------------------------------
select * from dbo.Users;
select * from dbo.UserPreferences;
select * from dbo.UserRoles;

--------------------------------------------
select * from dbo.Clients;
select * from dbo.ClientAddresses;
select * from dbo.ClientContactPersons;
select * from dbo.ClientDocuments;
select * from dbo.ClientFiscalData;

------------------------------
select * from dbo.Employees;
select * from dbo.EmployeeAddresses;
select * from dbo.EmployeeContactPersons;
select * from dbo.EmployeeFiscalData;
select * from dbo.EmployeeTeam;
------------------------------

select * from dbo.Visits;
select * from dbo.VisitAddresses;
select * from dbo.VisitAttachments;
select * from dbo.VisitContactPersons;
select * from dbo.VisitTeam;
select * from dbo.VisitTeamEmployee;
select * from dbo.VisitTeamEquipment;
select * from dbo.VisitTeamFunctions;
select * from dbo.VisitTeamVehicle;


select * from dbo.EquipmentTypes;			--
select * from dbo.Equipments;				--

select * from dbo.Vehicles;					--

select * from dbo.FileTypes;


select * from dbo.RolePermissions;
select * from dbo.JwtKeys;
select * from dbo.JobDefinitions;

--FOR JSON AUTO;

/*

*/


