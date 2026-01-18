/* ============================================================
   SCRIPT: Drop-All-Tables-And-RLS.sql
   OBJETIVO: Remover RLS, funções e todas as tabelas
   ============================================================ */

/* =========================
   REMOÇÃO DO ROW LEVEL SECURITY
   ========================= */

DROP SECURITY POLICY IF EXISTS dbo.TenantSecurityPolicy;        -- Remove a policy de Row Level Security
GO
DROP FUNCTION IF EXISTS dbo.fn_TenantAccessPredicate;           -- Remove a função usada pelo RLS
GO

/* =========================
   REMOÇÃO DAS TABELAS (ORDEM CORRETA)
   ========================= */

DROP TABLE IF EXISTS dbo.JwtKeys;
DROP TABLE IF EXISTS dbo.UserRoles;
DROP TABLE IF EXISTS dbo.RolePermissions;
DROP TABLE IF EXISTS dbo.Actions;
DROP TABLE IF EXISTS dbo.Resources;
DROP TABLE IF EXISTS dbo.Roles;
DROP TABLE IF EXISTS dbo.Users;

DROP TABLE IF EXISTS dbo.InterventionAddresses;
DROP TABLE IF EXISTS dbo.InterventionContacts;
DROP TABLE IF EXISTS dbo.Interventions;
DROP TABLE IF EXISTS dbo.Vehicles;
DROP TABLE IF EXISTS dbo.Equipments;
DROP TABLE IF EXISTS dbo.TeamMemberAddresses;
DROP TABLE IF EXISTS dbo.TeamMemberContacts;
DROP TABLE IF EXISTS dbo.TeamMembers;
DROP TABLE IF EXISTS dbo.ClientFiscalData;
DROP TABLE IF EXISTS dbo.ClientAddresses;
DROP TABLE IF EXISTS dbo.ClientContacts;
DROP TABLE IF EXISTS dbo.Clients;

DROP TABLE IF EXISTS dbo.Subscriptions;
DROP TABLE IF EXISTS dbo.TenantFiscalData;
DROP TABLE IF EXISTS dbo.TenantContacts;
DROP TABLE IF EXISTS dbo.TenantAddresses;
DROP TABLE IF EXISTS dbo.Tenants;
DROP TABLE IF EXISTS dbo.Plans;
GO
