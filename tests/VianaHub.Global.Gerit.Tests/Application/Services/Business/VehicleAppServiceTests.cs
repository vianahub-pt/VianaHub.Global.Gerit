using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moq;
using VianaHub.Global.Gerit.Application.Services.Business;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.Vehicle;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.Interfaces;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;
using Xunit;

namespace VianaHub.Global.Gerit.Tests.Application.Services.Business
{
    public class VehicleAppServiceTests
    {
        [Fact]
        public async Task BulkUploadAsync_ShouldReturnFalse_WhenNoFile()
        {
            var repoMock = new Mock<IVehicleDataRepository>();
            var domainMock = new Mock<IVehicleDomainService>();
            var mapperMock = new Mock<IMapper>();
            var notifyMock = new Mock<INotify>();
            var localizationMock = new Mock<ILocalizationService>();
            var currentUserMock = new Mock<ICurrentUserService>();

            var service = new VehicleAppService(repoMock.Object, domainMock.Object, mapperMock.Object, notifyMock.Object, localizationMock.Object, currentUserMock.Object);

            var result = await service.BulkUploadAsync(null, CancellationToken.None);

            Assert.False(result);
            notifyMock.Verify(n => n.Add(It.IsAny<string>(), 400), Times.Once);
        }

        [Fact]
        public async Task BulkUploadAsync_ShouldProcessValidCsv()
        {
            var repoMock = new Mock<IVehicleDataRepository>();
            var domainMock = new Mock<IVehicleDomainService>();
            var mapperMock = new Mock<IMapper>();
            var notifyMock = new Mock<INotify>();
            var localizationMock = new Mock<ILocalizationService>();
            var currentUserMock = new Mock<ICurrentUserService>();

            currentUserMock.Setup(c => c.GetTenantId()).Returns(1);

            // build CSV content
            var csv = new StringBuilder();
            csv.AppendLine("Plate;Brand;Model;Year;Color;FuelType");
            csv.AppendLine("ABC-1234;Ford;Focus;2018;Red;Gasoline");
            csv.AppendLine("XYZ-9876;Toyota;Corolla;2020;Blue;Diesel");

            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            var stream = new MemoryStream(bytes);

            var formFile = new FormFile(stream, 0, bytes.Length, "file", "vehicles.csv")
            {
                Headers = new HeaderDictionary(),
                ContentType = "text/csv"
            };

            repoMock.Setup(r => r.ExistsByPlateAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
            domainMock.Setup(d => d.CreateAsync(It.IsAny<Domain.Entities.Business.VehicleEntity>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var service = new VehicleAppService(repoMock.Object, domainMock.Object, mapperMock.Object, notifyMock.Object, localizationMock.Object, currentUserMock.Object);

            var result = await service.BulkUploadAsync(formFile, CancellationToken.None);

            Assert.True(result);
            repoMock.Verify(r => r.ExistsByPlateAsync(1, "ABC-1234", It.IsAny<CancellationToken>()), Times.Once);
            repoMock.Verify(r => r.ExistsByPlateAsync(1, "XYZ-9876", It.IsAny<CancellationToken>()), Times.Once);
            domainMock.Verify(d => d.CreateAsync(It.IsAny<Domain.Entities.Business.VehicleEntity>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        }
    }
}
