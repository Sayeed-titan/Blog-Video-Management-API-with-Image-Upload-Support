using MasterDetails.API.DTOs;
using MasterDetails.API.Entities;

namespace MasterDetails.API.Interfaces
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(UserDto request);
        Task<TokenResponseDto?> LoginAsync(LoginDto request);
        Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request);

    }
}
