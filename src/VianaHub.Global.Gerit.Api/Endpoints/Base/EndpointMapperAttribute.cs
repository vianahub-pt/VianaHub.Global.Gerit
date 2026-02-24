namespace VianaHub.Global.Gerit.Api.Endpoints.Base;

/// <summary>
/// Atributo marcador para descoberta automática de endpoints via reflexão.
/// Deve ser aplicado em classes estáticas que contêm métodos de extensão
/// para mapear endpoints (ex: MapActionEndpoints, MapUserEndpoints, etc).
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class EndpointMapperAttribute : Attribute
{
    /// <summary>
    /// Nome do método de mapeamento. Se não especificado, será inferido
    /// automaticamente baseado no nome da classe.
    /// </summary>
    public string MethodName { get; set; }
}
