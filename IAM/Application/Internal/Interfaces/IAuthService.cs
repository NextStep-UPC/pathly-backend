using pathly_backend.IAM.Application.Internal.DTOs;

namespace pathly_backend.IAM.Application.Internal.Interfaces

{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
    }
}