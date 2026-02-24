EXEC sp_set_session_context @key=N'IsSuperAdmin', @value=1;
select * from dbo.Plans;				--
select * from dbo.Subscriptions;		--
select * from dbo.Tenants;				--
select * from dbo.Users;				--
select * from dbo.Actions;				--
select * from dbo.Resources;			--
select * from dbo.Roles;				--
select * from dbo.UserRoles;			--
select * from dbo.RolePermissions order by ResourceId;		--
select * from dbo.JwtKeys;
------------------------------
select * from dbo.StatusTypes;				--
select * from dbo.Status;					--
select * from dbo.AddressTypes;				--
select * from dbo.Clients;					--
select * from dbo.ClientAddresses;			--
select * from dbo.ClientContacts;			--
select * from dbo.Functions;				--
select * from dbo.EquipmentTypes;			--
select * from dbo.Equipments;				--
select * from dbo.TeamMembers;				--
select * from dbo.TeamMemberAddresses;		--
select * from dbo.TeamMemberContacts;		--
select * from dbo.Interventions;			--
select * from dbo.InterventionAddresses;	--
select * from dbo.InterventionContacts;		--
select * from dbo.Vehicles;					--
select * from dbo.Teams;					--
select * from dbo.TeamMembersTeams;			--
select * from dbo.InterventionTeams;		--
select * from dbo.InterventionTeamVehicles;	--
select * from dbo.InterventionTeamEquipments;

select * from dbo.FileTypes;
select * from dbo.PlanFileRules;
select * from dbo.AttachmentCategories;
select * from dbo.InterventionAttachments;

FOR JSON AUTO;




/*
Estado do equipamento	1 - Disponível, Em uso, Em manutenção
Estado da intervenção	2 - Aberto, Pendente, Fechado, 
Estado do veículo		3 - Novo, Usado, Em Manutenção


TeamMembers
TeamMember
ActivateTeamMemberValidator
CreateTeamMemberValidator
DeactivateTeamMemberValidator

InterventionAddresses
InterventionContacts
InterventionStatus
Interventions

*/
