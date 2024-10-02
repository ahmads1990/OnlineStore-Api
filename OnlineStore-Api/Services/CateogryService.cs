using OnlineStore_Api.Models;

namespace OnlineStore_Api.Services;

public class CateogryService : ICateogryService
{
    private readonly CategoryRepo _categoryRepo;
    public CateogryService(CategoryRepo categoryRepo)
    {
        _categoryRepo = categoryRepo;
    }
    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return await _categoryRepo.GetAllCategoriesAsync();
    }
    public async Task<Category?> GetCategoryWithIDAsync(int categoryID)
    {
        return await _categoryRepo.GetCategoryWithIDAsync(categoryID);
    }
    public async Task<Category?> GetCategoryWithNameAsync(string categoryTitle)
    {
        var nameQuery = categoryTitle.ToLower();
        return await _categoryRepo.GetCategoryWithNameAsync(nameQuery);
    }
    public async Task<bool> CheckCategoryExistAsync(int categoryID)
    {
        if (categoryID <= 0) 
            return false;

        return await _categoryRepo.CheckCategoryExistAsync(categoryID);
    }
    public async Task<Category> AddNewCategoryAsync(Category category)
    {
        if (category == null || string.IsNullOrEmpty(category.Title) || string.IsNullOrEmpty(category.Description))
            throw new Exception();

        return await _categoryRepo.AddNewCategoryAsync(category);
    }
    public async Task<Category> UpdateCategoryAsync(Category category)
    {
        if (category == null || string.IsNullOrEmpty(category.Title) || string.IsNullOrEmpty(category.Description))
            throw new Exception();

        return await _categoryRepo.UpdateCategoryAsync(category);
    }
    public async Task<bool?> DeleteCategoryAsync(int categoryID)
    {
        if (categoryID <= 0) 
            return null;

        return await _categoryRepo.DeleteCategoryAsync(categoryID);
    }
}
