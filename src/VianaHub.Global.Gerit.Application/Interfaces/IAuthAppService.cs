using VianaHub.Global.Gerit.Application.Dtos.Request.Auth;
using VianaHub.Global.Gerit.Application.Dtos.Response.Auth;

namespace VianaHub.Global.Gerit.Application.Interfaces;

public interface IAuthAppService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken ct);
    Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct);
    Task<AuthResponse> RefreshAsync(RefreshRequest request, CancellationToken ct);
}
