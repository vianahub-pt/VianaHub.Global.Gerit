using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.Client;
using VianaHub.Global.Gerit.Application.Interfaces.Common;
using VianaHub.Global.Gerit.Application.Services.Business;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Enums;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;
using Xunit;

namespace VianaHub.Global.Gerit.Tests.Application.Services.Business;

public class ClientAppServiceTests
{
    [Fact]
    public async Task GetByIdAsync_ShouldReturnNullAndNotify_WhenAggregateDoesNotExist()
    {
        var repoMock = new Mock<IClientRepository>();
        var domainMock = new Mock<IClientDomainService>();
        var mapperMock = new Mock<IMapper>();
        var notifyMock = new Mock<INotify>();
        var localizationMock = new Mock<ILocalizationService>();
        var currentUserMock = new Mock<ICurrentUserService>();
        var fileValidationMock = new Mock<IFileValidationService>();
        var loggerMock = new Mock<ILogger<ClientAppService>>();

        currentUserMock.Setup(x => x.GetTenantId()).Returns(7);
        localizationMock.Setup(x => x.GetMessage("Application.Service.Client.GetById.ResourceNotFound")).Returns("not-found");
        repoMock.Setup(x => x.GetAggregateByIdAsync(7, 12, It.IsAny<CancellationToken>())).ReturnsAsync((ClientEntity)null);

        var service = new ClientAppService(
            repoMock.Object,
            domainMock.Object,
            mapperMock.Object,
            notifyMock.Object,
            localizationMock.Object,
            currentUserMock.Object,
            fileValidationMock.Object,
            loggerMock.Object);

        var result = await service.GetByIdAsync(12, CancellationToken.None);

        Assert.Null(result);
        notifyMock.Verify(x => x.Add("not-found", 410), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnFalse_WhenEmailAlreadyExistsInTenant()
    {
        var repoMock = new Mock<IClientRepository>();
        var domainMock = new Mock<IClientDomainService>();
        var mapperMock = new Mock<IMapper>();
        var notifyMock = new Mock<INotify>();
        var localizationMock = new Mock<ILocalizationService>();
        var currentUserMock = new Mock<ICurrentUserService>();
        var fileValidationMock = new Mock<IFileValidationService>();
        var loggerMock = new Mock<ILogger<ClientAppService>>();

        currentUserMock.Setup(x => x.GetTenantId()).Returns(3);
        localizationMock.Setup(x => x.GetMessage("Application.Service.Client.Create.ResourceAlreadyExists")).Returns("duplicate-email");
        repoMock.Setup(x => x.ExistsByEmailAsync(3, "client@gerit.test", It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var service = new ClientAppService(
            repoMock.Object,
            domainMock.Object,
            mapperMock.Object,
            notifyMock.Object,
            localizationMock.Object,
            currentUserMock.Object,
            fileValidationMock.Object,
            loggerMock.Object);

        var request = new CreateClientRequest
        {
            ClientType = (int)ClientType.PessoaSingular,
            Origin = (int)Origin.Outros,
            Name = "Client Test",
            Phone = "999999999",
            Email = "client@gerit.test",
            Consent = true,
            ConsentDate = DateTime.UtcNow
        };

        var result = await service.CreateAsync(request, CancellationToken.None);

        Assert.False(result);
        notifyMock.Verify(x => x.Add("duplicate-email", 409), Times.Once);
        domainMock.Verify(x => x.CreateAsync(It.IsAny<ClientEntity>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
