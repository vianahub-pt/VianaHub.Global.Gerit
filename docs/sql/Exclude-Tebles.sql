/* =========================================================
   EXCLUDE / DROP SCRIPT
   Atualizado com base no Create-Tables.sql atual.

   Ordem:
   1. Remove a Security Policy de Row-Level Security.
   2. Remove a função usada pela RLS.
   3. Remove as tabelas em ordem inversa de dependência.

   Observação:
   - Os índices, constraints, defaults e FKs são removidos
     automaticamente com suas respectivas tabelas.
   - O script foi estruturado para SQL Server 2016+.
   ========================================================= */

SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

BEGIN TRY
    BEGIN TRANSACTION;

    /* =========================================================
       1. REMOVER ROW-LEVEL SECURITY
       ========================================================= */

    IF EXISTS (
        SELECT 1
        FROM sys.security_policies
        WHERE name = N'TenantSecurityPolicy'
          AND schema_id = SCHEMA_ID(N'dbo')
    )
    BEGIN
        DROP SECURITY POLICY dbo.TenantSecurityPolicy;
    END;

    /* =========================================================
       2. REMOVER FUNÇÃO UTILIZADA PELA RLS
       ========================================================= */

    IF OBJECT_ID(N'dbo.fn_TenantAccessPredicate', N'IF') IS NOT NULL
    BEGIN
        DROP FUNCTION dbo.fn_TenantAccessPredicate;
    END;

    /* =========================================================
       3. REMOVER TABELAS EM ORDEM INVERSA DE DEPENDÊNCIA
       ========================================================= */

    -- Visitas e alocações
    DROP TABLE IF EXISTS dbo.VisitAttachments;
    DROP TABLE IF EXISTS dbo.VisitTeamEquipment;
    DROP TABLE IF EXISTS dbo.VisitTeamVehicle;
    DROP TABLE IF EXISTS dbo.VisitTeamEmployee;
    DROP TABLE IF EXISTS dbo.VisitTeamFunctions;
    DROP TABLE IF EXISTS dbo.VisitTeam;
    DROP TABLE IF EXISTS dbo.VisitAddresses;
    DROP TABLE IF EXISTS dbo.VisitContactPersons;
    DROP TABLE IF EXISTS dbo.Visits;

    -- Veículos, equipamentos e colaboradores
    DROP TABLE IF EXISTS dbo.Vehicles;
    DROP TABLE IF EXISTS dbo.Equipments;
    DROP TABLE IF EXISTS dbo.EquipmentTypes;
    DROP TABLE IF EXISTS dbo.EmployeeTeam;
    DROP TABLE IF EXISTS dbo.EmployeeFiscalData;
    DROP TABLE IF EXISTS dbo.EmployeeAddresses;
    DROP TABLE IF EXISTS dbo.EmployeeContactPersons;
    DROP TABLE IF EXISTS dbo.Employees;
    DROP TABLE IF EXISTS dbo.Teams;

    -- Clientes
    DROP TABLE IF EXISTS dbo.ClientFiscalData;
    DROP TABLE IF EXISTS dbo.ClientDocuments;
    DROP TABLE IF EXISTS dbo.ClientContactPersons;
    DROP TABLE IF EXISTS dbo.ClientAddresses;
    DROP TABLE IF EXISTS dbo.Clients;

    -- Jobs, autenticação e autorização
    DROP TABLE IF EXISTS dbo.JobDefinitions;
    DROP TABLE IF EXISTS dbo.JwtKeys;
    DROP TABLE IF EXISTS dbo.RefreshTokens;
    DROP TABLE IF EXISTS dbo.UserRoles;
    DROP TABLE IF EXISTS dbo.RolePermissions;
    DROP TABLE IF EXISTS dbo.Actions;
    DROP TABLE IF EXISTS dbo.Resources;
    DROP TABLE IF EXISTS dbo.Roles;
    DROP TABLE IF EXISTS dbo.UserPreferences;
    DROP TABLE IF EXISTS dbo.Users;

    -- Assinaturas e status tenant-aware
    DROP TABLE IF EXISTS dbo.Subscriptions;
    DROP TABLE IF EXISTS dbo.StatusDefinitionTranslations;
    DROP TABLE IF EXISTS dbo.StatusDefinitions;

    -- Dados do tenant
    DROP TABLE IF EXISTS dbo.TenantDocuments;
    DROP TABLE IF EXISTS dbo.TenantFiscalData;
    DROP TABLE IF EXISTS dbo.TenantAddresses;
    DROP TABLE IF EXISTS dbo.TenantContactPersons;
    DROP TABLE IF EXISTS dbo.Tenants;

    -- Planos e regras de arquivo
    DROP TABLE IF EXISTS dbo.SubscriptionPlanFileRules;
    DROP TABLE IF EXISTS dbo.SubscriptionPlanTranslations;
    DROP TABLE IF EXISTS dbo.SubscriptionPlans;

    -- Catálogos globais e traduções
    DROP TABLE IF EXISTS dbo.StatusDomainTranslations;
    DROP TABLE IF EXISTS dbo.StatusDomains;
    DROP TABLE IF EXISTS dbo.FileTypeTranslations;
    DROP TABLE IF EXISTS dbo.FileTypes;
    DROP TABLE IF EXISTS dbo.DocumentTypeTranslations;
    DROP TABLE IF EXISTS dbo.DocumentTypes;
    DROP TABLE IF EXISTS dbo.AddressTypeTranslations;
    DROP TABLE IF EXISTS dbo.AddressTypes;
    DROP TABLE IF EXISTS dbo.AcquisitionSourceTypeTranslations;
    DROP TABLE IF EXISTS dbo.AcquisitionSourceTypes;
    DROP TABLE IF EXISTS dbo.PartyTypeTranslations;
    DROP TABLE IF EXISTS dbo.PartyTypes;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;
GO
