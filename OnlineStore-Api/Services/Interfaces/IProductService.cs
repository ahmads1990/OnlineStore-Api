using OnlineStore_Api.Dtos;
using OnlineStore_Api.Helpers;

namespace OnlineStore_Api.Services.Interfaces;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllProductsAsync(int? maxLimit);
    Task<IEnumerable<Product>> GetAllProductsAsync(ProductSearchFilter searchFilter);
    Task<Product?> GetFullProductByIDAsync(int productID);
    Task<Product> AddNewProductAsync(Product product);
    Task<Product> UpdateProductAsync(Product product);
    Task<bool?> DeleteProductAsync(int productID);
}
