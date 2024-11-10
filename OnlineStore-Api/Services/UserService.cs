
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace OnlineStore_Api.Services;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;

    public UserService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IEnumerable<AppUser>> GetAllUsers()
    {
        return await _userManager.Users.ToListAsync();
    }

    public async Task<AppUser?> GetUserById(string userId)
    {
        return await _userManager.Users.FirstOrDefaultAsync(u => u.Id.Equals(userId));
    }
}
