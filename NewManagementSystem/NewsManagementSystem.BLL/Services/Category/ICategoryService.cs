namespace NewsManagementSystem.BLL.Services.Category;

using BusinessObject.Entities;

public interface ICategoryService
{
    Task<List<Category>> GetCategoriesAsync();
    Task<Category?> GetCategoryByNameAsync(string name);
    Task<Category?> GetCategoryByIdAsync(short categoryId);
    Task CreateCategoryAsync(Category category);
    Task UpdateCategoryAsync(Category category);
    Task DeleteCategoryAsync(Category category);
    Task<bool> CategoryExistsAsync(short categoryId);
    Task<List<Category>> SearchCategoriesByNameAsync(string searchTerm);

}