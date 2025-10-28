using EVChargingStation.CARC.Domain.HoaHTT.DTOs.AuthDTOs;
using Microsoft.Extensions.Configuration;

namespace EVChargingStation.CARC.Application.HoaHTT.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponceDto> LoginAsync(LoginRequestDto loginDto, IConfiguration configuration);
        Task<bool> LogoutAsync(Guid userId);
        Task<LoginResponceDto?> RefreshTokenAsync(TokenRefreshRequestDto refreshTokenDto, IConfiguration configuration);
    }
}
