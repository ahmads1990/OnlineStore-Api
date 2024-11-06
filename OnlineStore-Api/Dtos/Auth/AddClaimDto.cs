using System.ComponentModel.DataAnnotations;

namespace OnlineStore_Api.Dtos.Auth;

public class AddClaimDto
{
    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public string ClaimType { get; set; } = string.Empty;
    [Required]
    public string ClaimValue { get; set; } = string.Empty;
}
