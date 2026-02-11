EXEC sp_set_session_context @key=N'IsSuperAdmin', @value=1;
select * from dbo.Plans;				--
select * from dbo.Subscriptions;		--
select * from dbo.Tenants;				--
select * from dbo.Users;				--
select * from dbo.Actions;				--
select * from dbo.Resources;			--
select * from dbo.Roles;				--
select * from dbo.UserRoles;			--
select * from dbo.RolePermissions order by ResourceId, ActionId;		--
select * from dbo.JwtKeys;
------------------------------
select * from dbo.AddressTypes;			--
select * from dbo.Vehicles;				--
select * from dbo.EquipmentTypes;		--
select * from dbo.Equipments;			--
select * from dbo.Functions;			--
select * from dbo.TeamMembers;			--
select * from dbo.TeamMemberAddresses;	--
select * from dbo.TeamMemberContacts;	--
select * from dbo.Clients;
select * from dbo.ClientAddresses;
select * from dbo.ClientContacts;
select * from dbo.ClientFiscalData;

FOR JSON AUTO;



--Tenants			- TenantId	= 4 - VianaHub Lda
--Users				- UserId	= 4 - Dener Viana
--Roles				- RoleId	= 2 - BackOffice
--UserRoles			- TenantId=4 | UserId=4 | RoleId=2
--RolePermissions	- TenanrId=4 | RoleId=2 | 



/*
TeamMembers
TeamMember
ActivateTeamMemberValidator
CreateTeamMemberValidator
DeactivateTeamMemberValidator



*/
