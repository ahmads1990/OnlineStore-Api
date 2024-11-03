using Microsoft.AspNetCore.Identity;

namespace OnlineStore_Api.Models;

public class AppUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly BirthDate { get; set; }
    public GenderEnum Gender { get; set; }
}
public enum GenderEnum
{
    M,F
}
