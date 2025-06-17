using NewsManagementSystem.BusinessObject.Entities;
using NewsManagementSystem.DAL.Repositories.Category;

namespace NewsManagementSystem.BLL.Services.Category;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepo _categoryRepo;

    public CategoryService(ICategoryRepo categoryRepo)
    {
        _categoryRepo = categoryRepo;
    }


    public async Task<List<BusinessObject.Entities.Category>> GetCategoriesAsync()
    {
        return await _categoryRepo.GetCategoriesAsync();
    }

    public async Task<BusinessObject.Entities.Category?> GetCategoryByNameAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Category name cannot be null or empty.", nameof(name));
        }

        return await _categoryRepo.GetCategoryByNameAsync(name);
    }

    public async Task CreateCategoryAsync(BusinessObject.Entities.Category category)
    {
        if (category == null)
        {
            throw new ArgumentNullException(nameof(category));
        }
        var categoryModel = new BusinessObject.Entities.Category
        {
            CategoryName = category.CategoryName,
            CategoryDesciption = category.CategoryDesciption,
            IsActive = true
        };

        await _categoryRepo.CreateCategoryAsync(categoryModel);

        categoryModel.ParentCategoryId = categoryModel.CategoryId;

        await _categoryRepo.UpdateCategoryAsync(categoryModel);
    }

    public async Task UpdateCategoryAsync(BusinessObject.Entities.Category category)
    {
        if (category == null)
        {
            throw new ArgumentNullException(nameof(category));
        }

        await _categoryRepo.UpdateCategoryAsync(category);
    }

    public async Task DeleteCategoryAsync(BusinessObject.Entities.Category category)
    {
        if (category == null)
        {
            throw new ArgumentNullException(nameof(category));
        }

        await _categoryRepo.DeleteCategoryAsync(category);
    }

    public async Task<BusinessObject.Entities.Category?> GetCategoryByIdAsync(short categoryId)
    {
        if (categoryId <= 0)
        {
            throw new ArgumentException("Category ID must be greater than zero.", nameof(categoryId));
        }

        return await _categoryRepo.GetCategoryByIdAsync(categoryId);
    }

    public async Task<bool> CategoryExistsAsync(short categoryId)
    {
        if (categoryId <= 0)
        {
            throw new ArgumentException("Category ID must be greater than zero.", nameof(categoryId));
        }

        return await _categoryRepo.CategoryExistsAsync(categoryId);
    }

    public async Task<List<BusinessObject.Entities.Category>> SearchCategoriesByNameAsync(string searchTerm)
    {
        return await _categoryRepo.SearchCategoriesByNameAsync(searchTerm);
    }
}