using System.ComponentModel;

namespace VianaHub.Global.Gerit.Domain.Enums;

public enum ServiceContext
{
    [Description("Outros")]
    OutroContexto = 0,

    [Description("Create")]
    Create = 1,

    [Description("Read")]
    Read = 2,

    [Description("Update")]
    Update = 3,

    [Description("Delete")]
    Delete = 4,

    [Description("Activate")]
    Activate = 5,

    [Description("Deactivate")]
    Deactivate = 6,

    [Description("Search")]
    Search = 7
}
