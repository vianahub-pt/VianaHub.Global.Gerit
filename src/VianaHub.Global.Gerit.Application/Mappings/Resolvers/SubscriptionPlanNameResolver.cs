using AutoMapper;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;
using VianaHub.Global.Gerit.Domain.Services;

namespace VianaHub.Global.Gerit.Application.Mappings.Resolvers;

/// <summary>
/// Resolve o nome traduzido de um SubscriptionPlan com base no idioma da requisição.
/// </summary>
public class SubscriptionPlanNameResolver : IValueResolver<SubscriptionPlanEntity, object, string?>
{
    private readonly IRequestLanguageContext _languageContext;

    public SubscriptionPlanNameResolver(IRequestLanguageContext languageContext)
    {
        _languageContext = languageContext;
    }

    public string? Resolve(SubscriptionPlanEntity source, object destination, string? destMember, ResolutionContext context)
    {
        return TranslationResolver.Resolve(
            source.Translations,
            _languageContext.LanguageCode,
            t => t.LanguageCode,
            t => t.Name);
    }
}

/// <summary>
/// Resolve a descrição traduzida de um SubscriptionPlan com base no idioma da requisição.
/// </summary>
public class SubscriptionPlanDescriptionResolver : IValueResolver<SubscriptionPlanEntity, object, string?>
{
    private readonly IRequestLanguageContext _languageContext;

    public SubscriptionPlanDescriptionResolver(IRequestLanguageContext languageContext)
    {
        _languageContext = languageContext;
    }

    public string? Resolve(SubscriptionPlanEntity source, object destination, string? destMember, ResolutionContext context)
    {
        return TranslationResolver.ResolveDescription(
            source.Translations,
            _languageContext.LanguageCode,
            t => t.LanguageCode,
            t => t.Description);
    }
}
