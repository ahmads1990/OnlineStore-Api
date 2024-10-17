using Microsoft.AspNetCore.Mvc;
using OnlineStore_Api.Dtos.Product.ProductImage;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore_Api.Dtos.Product;

public class AddProductDto
{
    [Required, Length(5, 20)]
    public string Name { get; set; } = string.Empty;
    [Required, Length(30, 200)]
    public string Description { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    [Required, Range(1, 1000)]
    public float Price { get; set; }

    // Relations
    [Required, Range(1, int.MaxValue)]
    public int CategoryId { get; set; }
    [ModelBinder(Name = "Images")]
    public IEnumerable<AddProductImageDto> ProductImageDtos { get; set; } = new List<AddProductImageDto>();
}
