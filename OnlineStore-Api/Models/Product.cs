﻿namespace OnlineStore_Api.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public float Price { get; set; }
    // Relations
    public int CategoryId { get; set; }
    public Category Category { get; set; } = default!;
    public IEnumerable<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
}
