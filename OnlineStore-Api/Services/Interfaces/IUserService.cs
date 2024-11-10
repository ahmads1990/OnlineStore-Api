namespace OnlineStore_Api.Services.Interfaces;

public interface IUserService
{
    Task<IEnumerable<AppUser>> GetAllUsers();
    Task<AppUser?> GetUserById(string userId);
}
