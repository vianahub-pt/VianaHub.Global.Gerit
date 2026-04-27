select * from dbo.AddressTypes;
select * from dbo.FileTypes;
select * from dbo.ConsentTypes;
select * from dbo.StatusTypes;
select * from dbo.Plans;
select * from dbo.PlanFileRules;
select * from dbo.Actions;
select * from dbo.Resources;			--
select * from dbo.Roles;				--


EXEC sp_set_session_context @key=N'IsSuperAdmin', @value=1;

select * from dbo.Tenants;				--
select * from dbo.TenantContacts;
select * from dbo.TenantAddresses;
select * from dbo.TenantFiscalData;
select * from dbo.Subscriptions;

select * from dbo.Users;				--
select * from dbo.UserPreferences;		--
select * from dbo.UserRoles;			-- T1, U1, R2 
select * from dbo.RolePermissions;		-- T1, R4, R1, A1
select * from dbo.JwtKeys;
select * from dbo.JobDefinitions;
------------------------------

select * from dbo.Clients;					--
select * from dbo.ClientIndividuals;		--
select * from dbo.ClientCompanies;			--
select * from dbo.ClientAddresses;			--
select * from dbo.ClientContacts;			--
select * from dbo.ClientFiscalData;
select * from dbo.ClientHierarchy;
select * from dbo.ClientConsents;
------------------------------
select * from dbo.Teams;			--
select * from dbo.Functions;				--

select * from dbo.EquipmentTypes;			--
select * from dbo.Equipments;				--

select * from dbo.Vehicles;					--

select * from dbo.FileTypes;
select * from dbo.PlanFileRules;
select * from dbo.AttachmentCategories;

FOR JSON AUTO;

/*

*/


