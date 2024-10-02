namespace OnlineStore_Api.Repositories.Interfaces;

public interface IProductRepo
{
    Task<IEnumerable<Product>> GetAllProductsAsync(int? maxLimit);
    //Task<IEnumerable<Product>> GetProductsWithPaginationAsync(string searchQuery, int pageNumber, int pageSize);
    Task<Product?> GetFullProductByIDAsync(int productID);
    Task<Product> AddNewProductAsync(Product product);
    Task<Product> UpdateProductAsync(Product product);
    Task<bool?> DeleteProductAsync(int productID);
}
