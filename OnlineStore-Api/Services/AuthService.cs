using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OnlineStore_Api.Dtos.Auth;
using OnlineStore_Api.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineStore_Api.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly JwtConfig _jwtConfig;
    public AuthService(UserManager<AppUser> userManager, IOptions<JwtConfig> jwtConfig)
    {
        _userManager = userManager;
        _jwtConfig = jwtConfig.Value;
    }

    public async Task<AuthDto> RegisterUser(RegisterDto registerDto)
    {
        // first check if user email is already exists in database
        if (await _userManager.FindByEmailAsync(registerDto.Email) is not null)
            return new AuthDto { Message = "Email already exists" };

        // check if username is already exists in database
        if (await _userManager.FindByNameAsync(registerDto.UserName) is not null)
            return new AuthDto { Message = "Username already exists" };

        // create new user object
        var user = new AppUser();
        registerDto.Adapt(user);

        var result = await _userManager.CreateAsync(user, registerDto.Password); if (!result.Succeeded)
        {
            string errorMessage = string.Empty;
            foreach (var error in result.Errors)
            {
                errorMessage += $"{error.Description} | ";
            }
            return new AuthDto { Message = errorMessage };
        }

        // user creation went ok then create token and send it back
        var jwtToken = await CreateJwtTokenAsync(user);

        return new AuthDto
        {
            IsAuthenticated = true,
            Username = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            Claims = new List<string>(),
            Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
            ExpiresOn = jwtToken?.ValidTo ?? DateTime.Now
        };
    }
    public async Task<AuthDto> LoginUser(LoginDto loginDto)
    {
        AuthDto authDto = new AuthDto();
        // return if email doesn't exist OR email+password don't match
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user is null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            authDto.Message = "Email or Password is incorrect!";
            return authDto;
        }

        var jwtToken = await CreateJwtTokenAsync(user);
        var claims = await _userManager.GetClaimsAsync(user);

        authDto.IsAuthenticated = true;
        authDto.Username = user.UserName ?? string.Empty;
        authDto.Email = user.Email ?? string.Empty;
        authDto.Claims = claims.Select(c => c.Type).ToList();
        authDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        authDto.ExpiresOn = jwtToken?.ValidTo ?? DateTime.Now;

        return authDto;
    }
    public async Task<string> AddClaim(AddClaimDto claimDto)
    {
        var user = await _userManager.FindByIdAsync(claimDto.UserId);

        //check first if user with that id exists
        if (user is null)
            return "Invalid user ID";

        // claim type exists in allowed types
        if (!CustomClaimTypes.ALLOWEDTYPES.Contains(claimDto.ClaimType))
            return "Invalid claim type not allowed";

        // check user claims to see if user has this claim already
        var claims = await _userManager.GetClaimsAsync(user);
        if (claims.FirstOrDefault(c => c.Type.Equals(claimDto.ClaimType)) != null)
            return "User already assigned to this claim";

        // try to add the claim to user
        var result = await _userManager.AddClaimAsync(user, new Claim(claimDto.ClaimType, claimDto.ClaimValue));

        return result.Succeeded ? string.Empty : "Something went wrong";
    }
    private async Task<JwtSecurityToken?> CreateJwtTokenAsync(AppUser user)
    {
        if (user is null) return null;
        // get user claims
        var userClaims = await _userManager.GetClaimsAsync(user);
        // create jwt claims
        var jwtClaims = new[]
        {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim("uid", user.Id)
        };
        // merge both claims lists and jwtClaims to allClaims
        var allClaims = jwtClaims.Union(userClaims);

        // specify the signing key and algorithm
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        // finally create the token
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwtConfig.Issuer,
            audience: _jwtConfig.Audience,
            claims: allClaims,
            expires: DateTime.Now.AddHours(_jwtConfig.DurationInHours),
            signingCredentials: signingCredentials
            );

        return jwtSecurityToken;
    }
}