using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using VianaHub.Global.Gerit.Api.Endpoints.Identity;
using Moq;
using VianaHub.Global.Gerit.Application.Interfaces.Identity;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;
using VianaHub.Global.Gerit.Api.Endpoints.Billing;
using VianaHub.Global.Gerit.Application.Interfaces.Billing;
using VianaHub.Global.Gerit.Application.Interfaces.Job;
using VianaHub.Global.Gerit.Api.Endpoints.Job;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;
using Xunit;

namespace VianaHub.Global.Gerit.Tests.Api.Endpoints
{
    public class EndpointsRegistrationTests
    {
        private sealed class TestEndpointRouteBuilder : IEndpointRouteBuilder
        {
            public TestEndpointRouteBuilder(IServiceProvider serviceProvider)
            {
                ServiceProvider = serviceProvider;
                DataSources = new List<EndpointDataSource>();
            }

            public IServiceProvider ServiceProvider { get; }
            public ICollection<EndpointDataSource> DataSources { get; }

            public IApplicationBuilder CreateApplicationBuilder()
            {
                return new ApplicationBuilder(ServiceProvider);
            }
        }

        private static IEndpointRouteBuilder CreateRouteBuilder()
        {
            var services = new ServiceCollection();
            services.AddRouting();
            services.AddLogging();

            // register mocks for app services and other dependencies used by endpoints
            services.AddSingleton(new Mock<INotify>().Object);
            services.AddSingleton(new Mock<IPlanAppService>().Object);
            services.AddSingleton(new Mock<ISubscriptionAppService>().Object);
            services.AddSingleton(new Mock<ITenantAppService>().Object);
            services.AddSingleton(new Mock<IRoleAppService>().Object);
            services.AddSingleton(new Mock<IResourceAppService>().Object);
            services.AddSingleton(new Mock<IActionAppService>().Object);
            services.AddSingleton(new Mock<IRolePermissionAppService>().Object);
            services.AddSingleton(new Mock<IUserRoleAppService>().Object);
            services.AddSingleton(new Mock<IJobAppService>().Object);
            services.AddSingleton(new Mock<IJwtKeyAppService>().Object);
            services.AddSingleton(new Mock<IAuthAppService>().Object);
            services.AddSingleton(new Mock<ITenantContactPersonAppService>().Object);
            services.AddSingleton(new Mock<ILocalizationService>().Object);

            var sp = services.BuildServiceProvider();
            return new TestEndpointRouteBuilder(sp);
        }

        private static Microsoft.AspNetCore.Builder.WebApplication CreateWebApplication()
        {
            var builder = WebApplication.CreateBuilder();
            builder.Services.AddLogging();
            builder.Services.AddRouting();

            builder.Services.AddSingleton(new Mock<INotify>().Object);
            builder.Services.AddSingleton(new Mock<IPlanAppService>().Object);
            builder.Services.AddSingleton(new Mock<ISubscriptionAppService>().Object);
            builder.Services.AddSingleton(new Mock<ITenantAppService>().Object);
            builder.Services.AddSingleton(new Mock<IRoleAppService>().Object);
            builder.Services.AddSingleton(new Mock<IResourceAppService>().Object);
            builder.Services.AddSingleton(new Mock<IActionAppService>().Object);
            builder.Services.AddSingleton(new Mock<IRolePermissionAppService>().Object);
            builder.Services.AddSingleton(new Mock<IUserRoleAppService>().Object);
            builder.Services.AddSingleton(new Mock<IJobAppService>().Object);
            builder.Services.AddSingleton(new Mock<IJwtKeyAppService>().Object);
            builder.Services.AddSingleton(new Mock<IAuthAppService>().Object);
            builder.Services.AddSingleton(new Mock<ITenantContactPersonAppService>().Object);
            builder.Services.AddSingleton(new Mock<ILocalizationService>().Object);

            var app = builder.Build();
            return app;
        }

        [Fact(DisplayName = "Endpoints - MapPlanEndpoints não lança exceção")]
        [Trait("Api", "")]
        public void MapPlanEndpoints_DoesNotThrow()
        {
            var routeBuilder = CreateRouteBuilder();

            var ex = Record.Exception(() => PlanEndpoint.MapPlanEndpoints(routeBuilder));

            Assert.Null(ex);
        }

        [Fact(DisplayName = "Endpoints - MapSubscriptionEndpoints não lança exceção")]
        [Trait("Api", "")]
        public void MapSubscriptionEndpoints_DoesNotThrow()
        {
            var routeBuilder = CreateRouteBuilder();
            var ex = Record.Exception(() => SubscriptionEndpoint.MapSubscriptionEndpoints(routeBuilder));
            Assert.Null(ex);
        }

        [Fact(DisplayName = "Endpoints - MapTenantEndpoints não lança exceção")]
        [Trait("Api", "")]
        public void MapTenantEndpoints_DoesNotThrow()
        {
            var routeBuilder = CreateRouteBuilder();
            var ex = Record.Exception(() => TenantEndpoint.MapTenantEndpoints(routeBuilder));
            Assert.Null(ex);
        }

        [Fact(DisplayName = "Endpoints - MapRoleEndpoints não lança exceção")]
        [Trait("Api", "")]
        public void MapRoleEndpoints_DoesNotThrow()
        {
            var routeBuilder = CreateRouteBuilder();
            var ex = Record.Exception(() => RoleEndpoint.MapRoleEndpoints(routeBuilder));
            Assert.Null(ex);
        }

        [Fact(DisplayName = "Endpoints - MapResourceEndpoints não lança exceção")]
        [Trait("Api", "")]
        public void MapResourceEndpoints_DoesNotThrow()
        {
            var routeBuilder = CreateRouteBuilder();
            var ex = Record.Exception(() => ResourceEndpoint.MapResourceEndpoints(routeBuilder));
            Assert.Null(ex);
        }

        [Fact(DisplayName = "Endpoints - MapActionEndpoints não lança exceção")]
        [Trait("Api", "")]
        public void MapActionEndpoints_DoesNotThrow()
        {
            var routeBuilder = CreateRouteBuilder();
            var ex = Record.Exception(() => ActionEndpoint.MapActionEndpoints(routeBuilder));
            Assert.Null(ex);
        }

        [Fact(DisplayName = "Endpoints - MapRolePermissionEndpoints não lança exceção")]
        [Trait("Api", "")]
        public void MapRolePermissionEndpoints_DoesNotThrow()
        {
            var routeBuilder = CreateRouteBuilder();
            var ex = Record.Exception(() => RolePermissionEndpoint.MapRolePermissionEndpoints(routeBuilder));
            Assert.Null(ex);
        }

        [Fact(DisplayName = "Endpoints - MapUserRoleEndpoints não lança exceção")]
        [Trait("Api", "")]
        public void MapUserRoleEndpoints_DoesNotThrow()
        {
            var routeBuilder = CreateRouteBuilder();
            var ex = Record.Exception(() => UserRoleEndpoint.MapUserRoleEndpoints(routeBuilder));
            Assert.Null(ex);
        }

        [Fact(DisplayName = "Endpoints - MapJobEndpoints não lança exceção")]
        [Trait("Api", "")]
        public void MapJobEndpoints_DoesNotThrow()
        {
            var routeBuilder = CreateRouteBuilder();
            var ex = Record.Exception(() => JobEndpoint.MapJobEndpoints(routeBuilder));
            Assert.Null(ex);
        }

        [Fact(DisplayName = "Endpoints - MapJwtKeyEndpoints não lança exceção")]
        [Trait("Api", "")]
        public void MapJwtKeyEndpoints_DoesNotThrow()
        {
            var routeBuilder = CreateRouteBuilder();
            var ex = Record.Exception(() => JwtKeyEndpoint.MapJwtKeyEndpoints(routeBuilder));
            Assert.Null(ex);
        }

        [Fact(DisplayName = "Endpoints - MapAuthEndpoints não lança exceção")]
        [Trait("Api", "")]
        public void MapAuthEndpoints_DoesNotThrow()
        {
            var routeBuilder = CreateRouteBuilder();
            var ex = Record.Exception(() => AuthEndpoint.MapAuthEndpoints(routeBuilder));
            Assert.Null(ex);
        }

        [Fact(DisplayName = "Endpoints - MapTenantContactEndpoints - does not throw")]
        [Trait("Api", "")]
        public void MapTenantContactEndpoints_DoesNotThrow()
        {
            var routeBuilder = CreateRouteBuilder();
            var ex = Record.Exception(() => TenantContactPersonEndpoint.MapTenantContactEndpoints(routeBuilder));
            Assert.Null(ex);
        }
    }
}
