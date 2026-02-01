using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moq;
using VianaHub.Global.Gerit.Application.Services.Business;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.Equipment;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.Interfaces;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;
using Xunit;

namespace VianaHub.Global.Gerit.Tests.Application.Services.Business
{
    public class EquipmentAppServiceTests
    {
        [Fact]
        public async Task BulkUploadAsync_ShouldReturnFalse_WhenNoFile()
        {
            var repoMock = new Mock<IEquipmentDataRepository>();
            var domainMock = new Mock<IEquipmentDomainService>();
            var mapperMock = new Mock<IMapper>();
            var notifyMock = new Mock<INotify>();
            var localizationMock = new Mock<ILocalizationService>();
            var currentUserMock = new Mock<ICurrentUserService>();

            var service = new EquipmentAppService(repoMock.Object, domainMock.Object, mapperMock.Object, notifyMock.Object, localizationMock.Object, currentUserMock.Object);

            var result = await service.BulkUploadAsync(null, CancellationToken.None);

            Assert.False(result);
            notifyMock.Verify(n => n.Add(It.IsAny<string>(), 400), Times.Once);
        }

        [Fact]
        public async Task BulkUploadAsync_ShouldProcessValidCsv()
        {
            var repoMock = new Mock<IEquipmentDataRepository>();
            var domainMock = new Mock<IEquipmentDomainService>();
            var mapperMock = new Mock<IMapper>();
            var notifyMock = new Mock<INotify>();
            var localizationMock = new Mock<ILocalizationService>();
            var currentUserMock = new Mock<ICurrentUserService>();

            currentUserMock.Setup(c => c.GetTenantId()).Returns(1);

            // build CSV content
            var csv = new StringBuilder();
            csv.AppendLine("Name;SerialNumber;TypeEquipament");
            csv.AppendLine("Equip1;SN001;1");
            csv.AppendLine("Equip2;SN002;0");

            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            var stream = new MemoryStream(bytes);

            var formFile = new FormFile(stream, 0, bytes.Length, "file", "equipments.csv")
            {
                Headers = new HeaderDictionary(),
                ContentType = "text/csv"
            };

            repoMock.Setup(r => r.ExistsByNameAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
            domainMock.Setup(d => d.CreateAsync(It.IsAny<Domain.Entities.Business.EquipmentEntity>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var service = new EquipmentAppService(repoMock.Object, domainMock.Object, mapperMock.Object, notifyMock.Object, localizationMock.Object, currentUserMock.Object);

            var result = await service.BulkUploadAsync(formFile, CancellationToken.None);

            Assert.True(result);
            repoMock.Verify(r => r.ExistsByNameAsync(1, "Equip1", It.IsAny<CancellationToken>()), Times.Once);
            repoMock.Verify(r => r.ExistsByNameAsync(1, "Equip2", It.IsAny<CancellationToken>()), Times.Once);
            domainMock.Verify(d => d.CreateAsync(It.IsAny<Domain.Entities.Business.EquipmentEntity>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        }
    }
}
