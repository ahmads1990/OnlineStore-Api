using System.ComponentModel.DataAnnotations;

namespace OnlineStore_Api.Dtos;

public class AddCategoryDto
{
    [Required, Length(5,50)]
    public string Title { get; set; } = string.Empty;
    [Required, MaxLength(200)]
    public string Description { get; set; } = string.Empty;
}
