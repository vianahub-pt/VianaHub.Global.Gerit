using System.ComponentModel;

namespace VianaHub.Global.Gerit.Domain.Enums;

public enum RelationshipType
{
    [Description("Headquarters")]   // Sede ou matriz de uma empresa, onde estão localizadas as principais operações, decisões estratégicas e administração central da empresa.
    Headquarters = 1,

    [Description("Branch")]         // Filial de uma empresa, que pode estar localizada em um local diferente da sede, mas ainda faz parte da mesma entidade legal e é controlada pela empresa-mãe.
    Branch = 2,

    [Description("Holding")]        // Empresa controladora que possui participação majoritária em outras empresas, exercendo controle sobre suas operações e decisões estratégicas.
    Holding = 3,

    [Description("Subsidiary")]     // Empresa controlada por outra empresa, onde a empresa controladora possui participação majoritária e exerce controle sobre suas operações e decisões estratégicas.
    Subsidiary = 4,

    [Description("Franchise")]      // Modelo de negócio onde uma empresa (franqueador) concede a outra empresa (franqueado) o direito de operar um negócio usando a marca, produtos e sistemas do franqueador, em troca de uma taxa ou royalties.
    Franchise = 5
}
