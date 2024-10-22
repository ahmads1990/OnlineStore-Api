using System.Linq.Expressions;

namespace OnlineStore_Api.Repositories.Interfaces;

public interface IProductRepo
{
    Task<IEnumerable<Product>> GetAllProductsAsync(int? maxLimit);
    Task<IEnumerable<Product>> GetAllProductsAsync
        (Expression<Func<Product, bool>> filter, int limit, int page, bool IncludeImages = false);
    Task<Product?> GetFullProductByIDAsync(int productID);
    Task<Product> AddNewProductAsync(Product product);
    Task<Product> UpdateProductAsync(Product product);
    Task<bool?> DeleteProductAsync(int productID);
}
