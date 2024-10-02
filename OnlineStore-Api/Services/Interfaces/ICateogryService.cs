namespace OnlineStore_Api.Services.Interfaces;

public interface ICateogryService
{  // Read
    public Task<IEnumerable<Category>> GetAllCategoriesAsync();
    public Task<Category?> GetCategoryWithIDAsync(int categoryID);
    public Task<Category?> GetCategoryWithNameAsync(string categoryTitle);

    public Task<bool> CheckCategoryExistAsync(int categoryID);
    // Create
    public Task<Category> AddNewCategoryAsync(Category category);
    // Update
    public Task<Category> UpdateCategoryAsync(Category category);
    // Delete
    public Task<bool?> DeleteCategoryAsync(int categoryID);
}
