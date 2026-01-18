using System.Threading.Tasks;
using VianaHub.Global.Gerit.Domain.Entities;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Domain;

public interface IRolePermissionDomainService
{
    Task<bool> CreateAsync(RolePermissionEntity entity);
}
