
using Mapster;
using Microsoft.IdentityModel.Tokens;
using OnlineStore_Api.Dtos;
using OnlineStore_Api.Helpers;
using OnlineStore_Api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace OnlineStore_Api.Services;

public class ProductService : IProductService
{
    private readonly IProductRepo _productRepo;
    public ProductService(IProductRepo productRepo)
    {
        _productRepo = productRepo;
    }
    public async Task<IEnumerable<Product>> GetAllProductsAsync(int? maxLimit)
    {
        return await _productRepo.GetAllProductsAsync(maxLimit);
    }
    public async Task<IEnumerable<Product>> GetAllProductsAsync(ProductSearchFilter searchFilter)
    {
        Expression<Func<Product, bool>> filter = product=>
            (!string.IsNullOrEmpty(searchFilter.Name) || product.Name.Contains(searchFilter.Name)) &&
            (!searchFilter.CategoryId.HasValue || product.CategoryId == searchFilter.CategoryId);


        int maxLimit = Math.Max(searchFilter.Limit ?? 50, 1);
        var page = Math.Min(searchFilter.Page ?? 0, 10) - 1;

        return await _productRepo.GetAllProductsAsync(filter, maxLimit, page, searchFilter.IncludeImages);
    }
    public async Task<Product?> GetFullProductByIDAsync(int productID)
    {
        return await _productRepo.GetFullProductByIDAsync(productID);
    }
    public async Task<Product> AddNewProductAsync(Product product)
    {
        var createdProduct = await _productRepo.AddNewProductAsync(product);
        return createdProduct;
    }
    public Task<Product> UpdateProductAsync(Product product)
    {
        throw new NotImplementedException();
    }
    public Task<bool?> DeleteProductAsync(int productID)
    {
        throw new NotImplementedException();
    }
}
