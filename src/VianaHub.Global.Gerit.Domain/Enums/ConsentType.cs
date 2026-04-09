using System.ComponentModel;

namespace VianaHub.Global.Gerit.Domain.Enums;

public enum ConsentType
{
    [Description("Privacy Policy")]
    PrivacyPolicy = 1,

    [Description("Marketing")]
    Marketing = 2,

    [Description("Terms Of Service")]
    TermsOfService = 3,

    [Description("Data Processing")]
    DataProcessing = 4,

    [Description("Cookies")]
    Cookies = 5
}
