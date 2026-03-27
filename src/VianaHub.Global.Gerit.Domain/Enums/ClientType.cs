using System.ComponentModel;

namespace VianaHub.Global.Gerit.Domain.Enums;

public enum ClientType : byte
{
    [Description("Individual")]
    Individual = 1,

    [Description("Company")]
    Company = 2,

    [Description("Freelancer")]
    Freelancer = 3,

    [Description("PublicEntity")]
    PublicEntity = 4,

    [Description("NonProfit")]
    NonProfit = 4,
}
