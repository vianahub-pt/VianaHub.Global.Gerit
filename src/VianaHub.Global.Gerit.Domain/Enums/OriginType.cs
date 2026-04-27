using System.ComponentModel;

namespace VianaHub.Global.Gerit.Domain.Enums;

public enum OriginType
{
    [Description("Outros")]
    Outros = 1,

    [Description("Instagram")]
    Instagram = 2,

    [Description("Facebook")]
    Facebook = 3,

    [Description("LinkedIn")]
    LinkedIn = 4,

    [Description("YouTube")] 
    YouTube = 5,

    [Description("WhatsApp")]
    WhatsApp = 6,

    [Description("TikTok")] 
    TikTok = 7,

    [Description("Google")] 
    Google = 8,

    [Description("Amigos")] 
    Amigos = 9,

    [Description("TV")]     
    Tv = 10,

    [Description("Rádio")]  
    Radio = 11,

    [Description("Jornal")] 
    Jornal = 12,

    [Description("Revista")]
    Revista = 13,

    [Description("Eventos")]
    Eventos = 14,
}
