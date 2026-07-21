using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.AcquisitionSourceType;
using VianaHub.Global.Gerit.Application.Mappings.Business;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using Xunit;

namespace VianaHub.Global.Gerit.Tests.Application.Mappings.Business;

public class AcquisitionSourceTypeMappingProfileTests
{
    private readonly IMapper _mapper;

    public AcquisitionSourceTypeMappingProfileTests()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAutoMapper(cfg => cfg.AddProfile<AcquisitionSourceTypeMappingProfile>());
        var provider = services.BuildServiceProvider();
        _mapper = provider.GetRequiredService<IMapper>();
    }

    [Fact(DisplayName = "AcquisitionSourceTypeMapping — Name e Description resolvidos de tradução pt-PT")]
    [Trait("Application", "Mapping")]
    public void Map_ShouldResolveNameAndDescriptionFromPtPtTranslation()
    {
        var entity = new AcquisitionSourceTypeEntity("INSTAGRAM", 1);
        entity.AddTranslation(new AcquisitionSourceTypeTranslationsEntity(0, "pt-PT", "Instagram PT", "Descrição PT"));
        entity.AddTranslation(new AcquisitionSourceTypeTranslationsEntity(0, "en-US", "Instagram EN", "Description EN"));
        typeof(AcquisitionSourceTypeEntity)
            .GetProperty("Id", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
            .SetValue(entity, 1);

        // Força cultura pt-PT
        var currentCulture = CultureInfo.CurrentCulture;
        var currentUiCulture = CultureInfo.CurrentUICulture;
        try
        {
            CultureInfo.CurrentCulture = new CultureInfo("pt-PT");
            CultureInfo.CurrentUICulture = new CultureInfo("pt-PT");

            var result = _mapper.Map<AcquisitionSourceTypeResponse>(entity);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Instagram PT", result.Name);
            Assert.Equal("Descrição PT", result.Description);
            Assert.True(result.IsActive);
        }
        finally
        {
            CultureInfo.CurrentCulture = currentCulture;
            CultureInfo.CurrentUICulture = currentUiCulture;
        }
    }

    [Fact(DisplayName = "AcquisitionSourceTypeMapping — Name e Description resolvidos de tradução en-US")]
    [Trait("Application", "Mapping")]
    public void Map_ShouldResolveNameAndDescriptionFromEnUsTranslation()
    {
        var entity = new AcquisitionSourceTypeEntity("INSTAGRAM", 1);
        entity.AddTranslation(new AcquisitionSourceTypeTranslationsEntity(0, "pt-PT", "Instagram PT", "Descrição PT"));
        entity.AddTranslation(new AcquisitionSourceTypeTranslationsEntity(0, "en-US", "Instagram EN", "Description EN"));
        typeof(AcquisitionSourceTypeEntity)
            .GetProperty("Id", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
            .SetValue(entity, 1);

        var currentCulture = CultureInfo.CurrentCulture;
        var currentUiCulture = CultureInfo.CurrentUICulture;
        try
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
            CultureInfo.CurrentUICulture = new CultureInfo("en-US");

            var result = _mapper.Map<AcquisitionSourceTypeResponse>(entity);

            Assert.NotNull(result);
            Assert.Equal("Instagram EN", result.Name);
            Assert.Equal("Description EN", result.Description);
        }
        finally
        {
            CultureInfo.CurrentCulture = currentCulture;
            CultureInfo.CurrentUICulture = currentUiCulture;
        }
    }

    [Fact(DisplayName = "AcquisitionSourceTypeMapping — fallback para pt-PT quando idioma não encontrado")]
    [Trait("Application", "Mapping")]
    public void Map_ShouldFallbackToPtPt_WhenLanguageNotFound()
    {
        var entity = new AcquisitionSourceTypeEntity("INSTAGRAM", 1);
        entity.AddTranslation(new AcquisitionSourceTypeTranslationsEntity(0, "pt-PT", "Instagram PT", "Descrição PT"));
        typeof(AcquisitionSourceTypeEntity)
            .GetProperty("Id", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
            .SetValue(entity, 1);

        var currentCulture = CultureInfo.CurrentCulture;
        var currentUiCulture = CultureInfo.CurrentUICulture;
        try
        {
            CultureInfo.CurrentCulture = new CultureInfo("fr-FR");
            CultureInfo.CurrentUICulture = new CultureInfo("fr-FR");

            var result = _mapper.Map<AcquisitionSourceTypeResponse>(entity);

            Assert.NotNull(result);
            Assert.Equal("Instagram PT", result.Name);
            Assert.Equal("Descrição PT", result.Description);
        }
        finally
        {
            CultureInfo.CurrentCulture = currentCulture;
            CultureInfo.CurrentUICulture = currentUiCulture;
        }
    }

    [Fact(DisplayName = "AcquisitionSourceTypeMapping — Name e Description nulos quando sem traduções")]
    [Trait("Application", "Mapping")]
    public void Map_ShouldReturnNullNameAndDescription_WhenNoTranslationsExist()
    {
        var entity = new AcquisitionSourceTypeEntity("INSTAGRAM", 1);
        typeof(AcquisitionSourceTypeEntity)
            .GetProperty("Id", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
            .SetValue(entity, 1);

        var result = _mapper.Map<AcquisitionSourceTypeResponse>(entity);

        Assert.NotNull(result);
        Assert.Null(result.Name);
        Assert.Null(result.Description);
    }
}
