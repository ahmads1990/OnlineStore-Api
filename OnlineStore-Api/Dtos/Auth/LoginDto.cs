using System.ComponentModel.DataAnnotations;

namespace OnlineStore_Api.Dtos.Auth;

public class LoginDto
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}
