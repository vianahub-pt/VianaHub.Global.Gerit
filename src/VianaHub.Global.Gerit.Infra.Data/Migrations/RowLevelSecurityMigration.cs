using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VianaHub.Global.Gerit.Infra.Data.Migrations;

/// <summary>
/// Migration para configuração de Row Level Security (RLS)
/// Esta migration deve ser executada após a criação das tabelas
/// Implementa a função de isolamento de tenants e a política de segurança
/// </summary>
public static class RowLevelSecurityMigration
{
    /// <summary>
    /// SQL para criar a função de acesso de tenant
    /// </summary>
    public static string CreateTenantAccessFunction => @"
CREATE FUNCTION fn_TenantAccessPredicate (
    @TenantId INT
)
RETURNS TABLE
WITH SCHEMABINDING
AS
RETURN
    SELECT 1 AS fn_access
    WHERE @TenantId = CAST(SESSION_CONTEXT(N'TenantId') AS INT);
";

    /// <summary>
    /// SQL para remover a função de acesso de tenant
    /// </summary>
    public static string DropTenantAccessFunction => @"
DROP FUNCTION IF EXISTS dbo.fn_TenantAccessPredicate;
";

    /// <summary>
    /// SQL para criar a política de segurança de Row Level Security
    /// </summary>
    public static string CreateSecurityPolicy => @"
CREATE SECURITY POLICY dbo.TenantSecurityPolicy
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Users,
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Roles,
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.UserRoles,
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.RolePermissions,
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.TenantContacts,
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.TenantAddresses,
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.TenantFiscalData,
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Clients,
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.ClientContacts,
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.ClientAddresses,
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.TeamMembers,
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Interventions,
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Vehicles,
ADD FILTER PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Equipments,

ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Users AFTER INSERT,
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Users AFTER UPDATE,
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Users BEFORE DELETE,
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Roles AFTER INSERT,
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Roles AFTER UPDATE,
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Roles BEFORE DELETE,
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.UserRoles AFTER INSERT,
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.UserRoles AFTER UPDATE,
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.UserRoles BEFORE DELETE,
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.RolePermissions AFTER INSERT,
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.RolePermissions AFTER UPDATE,
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.RolePermissions BEFORE DELETE,
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.TenantContacts AFTER INSERT,
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.TenantContacts AFTER UPDATE,
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.TenantContacts BEFORE DELETE,
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.TenantAddresses AFTER INSERT,
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.TenantAddresses AFTER UPDATE,
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.TenantAddresses BEFORE DELETE,
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.TenantFiscalData AFTER INSERT,
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.TenantFiscalData AFTER UPDATE,
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.TenantFiscalData BEFORE DELETE,
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Clients AFTER INSERT,
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Clients AFTER UPDATE,
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Clients BEFORE DELETE,
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.ClientContacts AFTER INSERT,
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.ClientContacts AFTER UPDATE,
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.ClientContacts BEFORE DELETE,
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.ClientAddresses AFTER INSERT,
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.ClientAddresses AFTER UPDATE,
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.ClientAddresses BEFORE DELETE,
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.TeamMembers AFTER INSERT,
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.TeamMembers AFTER UPDATE,
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.TeamMembers BEFORE DELETE,
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Interventions AFTER INSERT,
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Interventions AFTER UPDATE,
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Interventions BEFORE DELETE,
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Vehicles AFTER INSERT,
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Vehicles AFTER UPDATE,
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Vehicles BEFORE DELETE,
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Equipments AFTER INSERT,
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Equipments AFTER UPDATE,
ADD BLOCK PREDICATE dbo.fn_TenantAccessPredicate(TenantId) ON dbo.Equipments BEFORE DELETE
WITH (STATE = ON);
";

    /// <summary>
    /// SQL para remover a política de segurança
    /// </summary>
    public static string DropSecurityPolicy => @"
DROP SECURITY POLICY IF EXISTS dbo.TenantSecurityPolicy;
";

    /// <summary>
    /// Aplica a configuração de Row Level Security
    /// </summary>
    public static void ApplyRowLevelSecurity(MigrationBuilder migrationBuilder)
    {
        // Criar função de acesso
        migrationBuilder.Sql(CreateTenantAccessFunction);

        // Criar política de segurança
        migrationBuilder.Sql(CreateSecurityPolicy);
    }

    /// <summary>
    /// Remove a configuração de Row Level Security
    /// </summary>
    public static void RemoveRowLevelSecurity(MigrationBuilder migrationBuilder)
    {
        // Remover política de segurança
        migrationBuilder.Sql(DropSecurityPolicy);

        // Remover função de acesso
        migrationBuilder.Sql(DropTenantAccessFunction);
    }
}
