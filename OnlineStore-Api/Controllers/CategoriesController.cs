using Mapster;
using Microsoft.AspNetCore.Mvc;
using OnlineStore_Api.Dtos.Category;

namespace OnlineStore_Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICateogryService _cateogryService;
    public CategoriesController(ICateogryService cateogryService)
    {
        _cateogryService = cateogryService;
    }
    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await _cateogryService.GetAllCategoriesAsync();
        return Ok(categories);
    }
    [HttpGet("{categoryID:int}")]
    public async Task<IActionResult> GetCategorieById(int categoryID)
    {
        var category = await _cateogryService.GetCategoryWithIDAsync(categoryID);

        if (category is null)
            return NotFound($"Couldn't find category with id {categoryID}");

        return Ok(category);
    }
    [HttpGet("{categoryName}")]
    public async Task<IActionResult> GetCategorieByName(string categoryName)
    {
        var category = await _cateogryService.GetCategoryWithNameAsync(categoryName);

        if (category is null)
            return NotFound($"Couldn't find category with name {categoryName}");

        return Ok(category);
    }
    [HttpPost]
    public async Task<IActionResult> AddNewCategory(AddCategoryDto categoryDto)
    {
        var inputCategory = categoryDto.Adapt<Category>();
        try
        {
            var addedCategory = await _cateogryService.AddNewCategoryAsync(inputCategory);
            return Ok(addedCategory);
        }
        catch (ArgumentException ex)
        {
            return BadRequest($"Invalid request: {ex.Message}");
        }
    }
    [HttpPut]
    public async Task<IActionResult> UpdateExistingCategory(Category category)
    {
        try
        {
            var updatedCategory = await _cateogryService.UpdateCategoryAsync(category);
            return Ok(updatedCategory);
        }
        catch (ArgumentException ex)
        {
            return BadRequest($"Invalid request: {ex.Message}");
        }
    }
    [HttpDelete("{categoryID:int}")]
    public async Task<IActionResult> DeleteExistingCategory(int categoryID)
    {
        try
        {
            var categoryDeleted = await _cateogryService.DeleteCategoryAsync(categoryID);
            if (categoryDeleted is null)
                return NotFound($"Couldn't find category with id {categoryID}");
            return Ok(categoryDeleted);
        }
        catch (ArgumentException ex)
        {
            return BadRequest($"Invalid request: {ex.Message}");
        }
    }
}
