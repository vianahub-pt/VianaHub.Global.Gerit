using System.ComponentModel;

namespace VianaHub.Global.Gerit.Domain.Enums;

/// <summary>
/// Status do Equipamento
/// </summary>
public enum EquipmentStatus : byte
{
    [Description("Disponível")]
    Available = 0,

    [Description("Em uso")]
    InUse = 1,

    [Description("Em manutenção")]
    InMaintenance = 2
}
