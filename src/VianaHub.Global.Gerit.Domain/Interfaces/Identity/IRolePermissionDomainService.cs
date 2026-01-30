using System.Threading.Tasks;
using VianaHub.Global.Gerit.Domain.Entities.Identity;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Identity;

public interface IRolePermissionDomainService
{
    Task<bool> CreateAsync(RolePermissionEntity entity);
}
