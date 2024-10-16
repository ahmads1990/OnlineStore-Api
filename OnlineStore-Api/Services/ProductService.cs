
using Mapster;
using OnlineStore_Api.Dtos;

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
