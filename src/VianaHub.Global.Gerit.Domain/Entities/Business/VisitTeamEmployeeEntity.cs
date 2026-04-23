using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Billing;

namespace VianaHub.Global.Gerit.Domain.Entities.Business;

/// <summary>
/// Entidade que representa um Funcionário associado a uma Equipe de Visita
/// Relaciona funcionários com suas funções específicas em visitas
/// </summary>
public class VisitTeamEmployeeEntity : Entity
{
    public int TenantId { get; private set; }
    public int VisitTeamId { get; private set; }
    public int EmployeeId { get; private set; }
    public int FunctionId { get; private set; }
    public bool IsLeader { get; private set; }
    public DateTime StartDateTime { get; private set; }
    public DateTime? EndDateTime { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Properties
    public TenantEntity Tenant { get; private set; }
    public VisitTeamEntity VisitTeam { get; private set; }
    public EmployeeEntity Employee { get; private set; }
    public FunctionEntity Function { get; private set; }

    protected VisitTeamEmployeeEntity() { }

    /// <summary>
    /// Construtor para criação de um novo Funcionário na Equipe de Visita
    /// </summary>
    public VisitTeamEmployeeEntity(int tenantId, int visitTeamId, int employeeId, int functionId, 
        bool isLeader, DateTime startDateTime, int createdBy)
    {
        TenantId = tenantId;
        VisitTeamId = visitTeamId;
        EmployeeId = employeeId;
        FunctionId = functionId;
        IsLeader = isLeader;
        StartDateTime = startDateTime;
        IsActive = true;
        IsDeleted = false;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(int functionId, bool isLeader, DateTime startDateTime, DateTime? endDateTime, int modifiedBy)
    {
        FunctionId = functionId;
        IsLeader = isLeader;
        StartDateTime = startDateTime;
        EndDateTime = endDateTime;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void SetEndDateTime(DateTime endDateTime, int modifiedBy)
    {
        if (endDateTime < StartDateTime)
            throw new InvalidOperationException("A data de término não pode ser anterior à data de início.");

        EndDateTime = endDateTime;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void SetAsLeader(int modifiedBy)
    {
        IsLeader = true;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void RemoveLeadership(int modifiedBy)
    {
        IsLeader = false;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Activate(int modifiedBy)
    {
        IsActive = true;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Deactivate(int modifiedBy)
    {
        IsActive = false;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Delete(int modifiedBy)
    {
        IsDeleted = true;
        IsActive = false;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }
}
