using System.ComponentModel;

namespace VianaHub.Global.Gerit.Domain.Enums;

public enum RiskLevel
{
    [Description("Low")]
    Low = 0,

    [Description("Medium")]
    Medium = 1,

    [Description("High")]
    High = 2,

    [Description("Critical")]
    Critical = 3
}
