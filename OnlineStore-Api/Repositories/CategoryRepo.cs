using Microsoft.EntityFrameworkCore;

namespace OnlineStore_Api.Repositories;

public class CategoryRepo : ICategoryRepo
{
    private AppDbContext _context { get; set; }
    public CategoryRepo(AppDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return await _context.Categories.ToListAsync();
    }
    public async Task<Category?> GetCategoryWithIDAsync(int categoryID)
    {
        return await _context.Categories
                        .FirstOrDefaultAsync(c => c.CategoryID == categoryID);
    }
    public async Task<Category?> GetCategoryWithNameAsync(string categoryTitle)
    {
        return await _context.Categories
                      .FirstOrDefaultAsync(c => c.Title == categoryTitle);
    }
    public async Task<bool> CheckCategoryExistAsync(int categoryID)
    {
        return await _context.Categories
                      .AnyAsync(c => c.CategoryID == categoryID);
    }
    public async Task<Category> AddNewCategoryAsync(Category category)
    {
        var newCategory = await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();

        return newCategory.Entity;
    }
    public async Task<Category> UpdateCategoryAsync(Category category)
    {
        var updatedCategory = _context.Categories.Update(category);
        await _context.SaveChangesAsync();

        return updatedCategory.Entity;
    }
    public async Task<bool?> DeleteCategoryAsync(int categoryID)
    {
        var category = await GetCategoryWithIDAsync(categoryID);
        if(category is not null)
        {
            _context.Categories.Remove(category);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        return null;
    }
}
