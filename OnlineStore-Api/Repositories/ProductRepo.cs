using Microsoft.EntityFrameworkCore;
using OnlineStore_Api.Repositories.Interfaces;
using System.Linq.Expressions;

namespace OnlineStore_Api.Repositories;

public class ProductRepo : IProductRepo
{
    private AppDbContext _context { get; set; }
    public ProductRepo(AppDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Product>> GetAllProductsAsync(int? maxLimit)
    {
        var query = _context.Products
                        .Include(p=>p.Category)
                        .Include(p=>p.ProductImages)
                        .AsQueryable();

        if (maxLimit is not null && maxLimit > 0)
            query = query.Take(maxLimit.Value);

        return await query.ToListAsync();
    }
    async Task<IEnumerable<Product>> GetAllProductsAsync
    (Expression<Func<Product, bool>> filter, int limit, int page, bool IncludeImages)
    {
        var query = _context.Products
                        .Include(p => p.Category)
                        .AsQueryable();

        if (IncludeImages)
            query = query.Include(p => p.ProductImages);

        query = query.Where(filter);
        query = query.Skip(page * limit).Take(limit);

        return await query.ToListAsync();
    }
    public async Task<Product?> GetFullProductByIDAsync(int productID)
    {
        return await _context.Products
                        .Include(p => p.Category)
                        .Include(p => p.ProductImages)
                        .FirstOrDefaultAsync(p => p.Id == productID);
    }
    public async Task<Product> AddNewProductAsync(Product product)
    {
        var newProduct =await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        return newProduct.Entity;
    }
    public async Task<Product> UpdateProductAsync(Product product)
    {
        var updatedProduct = _context.Products.Update(product);
        await _context.SaveChangesAsync();

        return updatedProduct.Entity;
    }
    public async Task<bool?> DeleteProductAsync(int productID)
    {
        var existingProduct = await GetFullProductByIDAsync(productID);
        if (existingProduct is not null)
        {
            _context.Products.Remove(existingProduct);
            var result =await _context.SaveChangesAsync();
            return result > 0;
        }
        return null;
    }
}
