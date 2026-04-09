select * from dbo.AddressTypes;
select * from dbo.FileTypes;
select * from dbo.ClientTypes;
select * from dbo.ConsentTypes;
select * from dbo.OriginTypes;
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
select * from dbo.UserRoles;			--
select * from dbo.RolePermissions order by ResourceId;		--
select * from dbo.JwtKeys;
select * from dbo.JobDefinitions;
------------------------------
select * from dbo.Clients;					--
select * from dbo.ClientIndividuals;
select * from dbo.ClientCompanies;
select * from dbo.ClientAddresses;			--
select * from dbo.ClientContacts;			--
select * from dbo.ClientIndividualFiscalData;
select * from dbo.ClientCompanyFiscalData;
select * from dbo.ClientHierarchy;
select * from dbo.ClientConsents;
------------------------------
select * from dbo.Teams;			--
select * from dbo.Functions;				--



select * from dbo.EquipmentTypes;			--
select * from dbo.Equipments;				--
select * from dbo.TeamMembers;				--
select * from dbo.TeamMembersTeams;			--
select * from dbo.TeamMemberAddresses;		--
select * from dbo.TeamMemberContacts;		--
select * from dbo.Interventions;			--
select * from dbo.InterventionAddresses;	--
select * from dbo.InterventionContacts;		--
select * from dbo.InterventionTeams;		--
select * from dbo.InterventionTeamVehicles;	--
select * from dbo.InterventionTeamEquipments;--
select * from dbo.Vehicles;					--

select * from dbo.FileTypes;
select * from dbo.PlanFileRules;
select * from dbo.AttachmentCategories;
select * from dbo.InterventionAttachments;

FOR JSON AUTO;

/*

*/


OriginTypes