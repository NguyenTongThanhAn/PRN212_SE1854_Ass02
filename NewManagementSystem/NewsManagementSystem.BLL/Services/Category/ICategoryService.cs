namespace NewsManagementSystem.BLL.Services.Category;

using BusinessObject.Entities;

public interface ICategoryService
{
    Task<List<Category>> GetCategoriesAsync();
    Task<Category?> GetCategoryByNameAsync(string name);
    Task<Category?> GetCategoryByIdAsync(int categoryId);
    Task CreateCategoryAsync(Category category);
    Task UpdateCategoryAsync(Category category);
    Task DeleteCategoryAsync(Category category);
    Task<bool> CategoryExistsAsync(int categoryId);
    Task<List<Category>> SearchCategoriesByNameAsync(string searchTerm);

}