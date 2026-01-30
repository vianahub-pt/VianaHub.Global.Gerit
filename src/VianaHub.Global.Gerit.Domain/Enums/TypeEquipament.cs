using System.ComponentModel;

namespace VianaHub.Global.Gerit.Domain.Enums;

/// <summary>
/// Tipos de Equipamento
/// </summary>
public enum TypeEquipament : byte
{
    [Description("Ferramenta Elétrica")]
    ElectricTool = 0,

    [Description("Ferramenta Manual")]
    ManualTool = 1,

    [Description("Medição")]
    Measurement = 2
}
