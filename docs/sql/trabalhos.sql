select * from dbo.AcquisitionSourceTypes;
select * from dbo.ConsentOriginTypes;
select * from dbo.AddressTypes;
select * from dbo.FileTypes;
select * from dbo.ConsentTypes;
select * from dbo.StatusTypes;
select * from dbo.Plans;
select * from dbo.PlanFileRules;
select * from dbo.Actions;
select * from dbo.Resources;			-- R32
select * from dbo.Roles;				-- R2


EXEC sp_set_session_context @key=N'IsSuperAdmin', @value=1;

select * from dbo.Tenants;				-- T1
select * from dbo.TenantContacts;
select * from dbo.TenantAddresses;
select * from dbo.TenantFiscalData;
select * from dbo.Subscriptions;

select * from dbo.Users;				-- U1
select * from dbo.UserPreferences;		--
select * from dbo.UserRoles;			-- T1, U1, R2 

select * from dbo.RolePermissions;		-- T1, R2, R32, A1
select * from dbo.RolePermissions 
where TenantId = 1
  and RoleId = 2
  and ResourceId = 32
select * from dbo.JwtKeys;
select * from dbo.JobDefinitions;
------------------------------

select * from dbo.Clients;
select * from dbo.ClientIndividuals;
select * from dbo.ClientCompanies;
select * from dbo.ClientContacts;
select * from dbo.ClientAddresses;
select * from dbo.ClientFiscalData;
select * from dbo.ClientConsents;
select * from dbo.ClientHierarchy;
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


