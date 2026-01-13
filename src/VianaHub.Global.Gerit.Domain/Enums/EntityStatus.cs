using System.ComponentModel;

namespace VianaHub.Global.Gerit.Domain.Enums;

public enum EntityStatus
{
    [Description("Unknown")]
    Unknown = 0,

    [Description("Active")]
    Active = 1,

    [Description("Inactive")]
    Inactive = 2,

    [Description("Canceled")]
    Canceled = 3,

    [Description("Deleted")]
    Deleted = 4
}
