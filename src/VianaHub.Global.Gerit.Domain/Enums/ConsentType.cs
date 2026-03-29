using System.ComponentModel;

namespace VianaHub.Global.Gerit.Domain.Enums;

public enum ConsentType
{
    [Description("Sem consentimento")]
    None = 1,

    [Description("Execução de contrato")]
    Contract = 2,

    [Description("Interesse legítimo")]
    LegitimateInterest = 3,

    [Description("Email marketing")]
    Marketing = 4,

    [Description("Newsletter")]
    Newsletter = 5,
    
    [Description("Processamento geral de dados")]
    DataProcessing = 6,
    
    [Description("Compartilhamento com terceiros")] 
    ThirdPartySharing = 7,
    
    [Description("Consentimento de cookies")]
    Cookies = 8,

    [Description("Análise de comportamento")]
    Profiling = 9
}
