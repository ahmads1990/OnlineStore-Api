using OnlineStore_Api.Dtos.Auth;

namespace OnlineStore_Api.Services.Interfaces;

public interface IAuthService
{
    Task<AuthDto> RegisterUser(RegisterDto registerDto);
    Task<AuthDto> LoginUser(LoginDto loginDto);
    Task<string> AddClaim(AddClaimDto claimDto);
} 
