using System.ComponentModel;

namespace VianaHub.Global.Gerit.Domain.Enums;

public enum ClientType
{
    [Description("Pessoa Singular")]                    // Sem atividade profissional aberta nas finanças, compra para uso próprio.
    PessoaSingular = 1,

    [Description("Recibos Verdes")]                     // Profissional que trabalha por conta própria, emitindo recibos verdes (prestação de serviços), sem estrutura empresarial complexa.
    RecibosVerdes = 2,

    [Description("Freelancer")]                         // Profissional independente que oferece serviços de forma autônoma, sem vínculo empregatício, podendo atuar em diversas áreas, como design, programação, redação, etc.
    Freelancer = 3,

    [Description("Pessoa Jurídica")]                    // Empresa constituída legalmente, com atividades comerciais, industriais ou de serviços, podendo ter funcionários e estrutura organizacional.
    PessoaJuridica = 4,

    [Description("Sociedade Unipessoal por Quotas")]    // Empresa constituída por um único sócio, com responsabilidade limitada ao capital social, onde o sócio é o único titular das quotas da empresa.
    SociedadeUnipessoalQuotas = 5,
}
