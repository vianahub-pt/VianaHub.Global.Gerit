EXEC sp_set_session_context @key=N'IsSuperAdmin', @value=1;
select * from dbo.Plans;			--
select * from dbo.Subscriptions;	--
select * from dbo.Tenants;			--
select * from dbo.Users;			--
select * from dbo.Actions;			--
select * from dbo.Resources;		--
select * from dbo.Roles;			--
select * from dbo.UserRoles;		--
select * from dbo.RolePermissions;	--
------------------------------
select * from dbo.Vehicles;			--
select * from dbo.Equipments;		--
select * from dbo.Functions;		--
select * from dbo.TeamMembers;
select * from dbo.Clients;

FOR JSON AUTO;


/*
TeamMembers
TeamMember
ActivateTeamMemberValidator
CreateTeamMemberValidator
DeactivateTeamMemberValidator



*/
