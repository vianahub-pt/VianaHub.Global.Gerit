using System.ComponentModel;

namespace VianaHub.Global.Gerit.Domain.Enums;

/// <summary>
/// Status possíveis para uma Intervenção
/// </summary>
public enum InterventionStatus : byte
{
    [Description("Pendente")]
    Pending = 0,

    [Description("Em Progresso")]
    InProgress = 1,

    [Description("Concluída")]
    Completed = 2,

    [Description("Cancelada")]
    Cancelled = 3,

    [Description("Pausada")]
    Paused = 4
}
