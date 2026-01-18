using System.Threading.Tasks;
using VianaHub.Global.Gerit.Domain.Entities;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Domain;

public interface IUserRoleDomainService
{
    Task<bool> CreateAsync(UserRoleEntity entity);
}
