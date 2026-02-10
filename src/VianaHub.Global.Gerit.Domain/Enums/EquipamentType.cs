using System.ComponentModel;

namespace VianaHub.Global.Gerit.Domain.Enums;

/// <summary>
/// Tipos de Equipamento
/// </summary>
public enum EquipamentType : byte
{
    [Description("Ferramenta Elétrica")]
    ElectricTool = 0,

    [Description("Ferramenta Manual")]
    ManualTool = 1,

    [Description("Medição")]
    Measurement = 2
}
