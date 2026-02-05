EXEC sp_set_session_context @key=N'IsSuperAdmin', @value=1;
select * from dbo.Plans;
select * from dbo.Subscriptions;
select * from dbo.Tenants;
select * from dbo.Users;
select * from dbo.Actions order by id;
select * from dbo.Resources;
select * from dbo.Roles;
select * from dbo.UserRoles;
select * from dbo.RolePermissions;
select * from dbo.Clients;
select * from dbo.JwtKeys;
select * from dbo.RefreshTokens order by ExpiresAt desc;
select * from dbo.JobDefinitions;
---------------------------------------
select * from dbo.Vehicles;
select * from dbo.Equipments;
select * from dbo.Functions;
select * from dbo.TeamMembers;
select * from dbo.Clients;

FOR JSON AUTO;


/*
delete dbo.Plans where id != 2;


Subscriptions
Subscription
subscriptions
subscription

Update dbo.Actions Set name = 'PostDeactivate' where id = 7;
delete dbo.Actions;
DBCC CHECKIDENT ('dbo.Actions', RESEED, 0);

delete dbo.RolePermissions;
DBCC CHECKIDENT ('dbo.RolePermissions', RESEED, 0);


*/
