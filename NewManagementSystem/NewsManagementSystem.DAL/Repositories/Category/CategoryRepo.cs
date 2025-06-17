using Microsoft.EntityFrameworkCore;
using NewsManagementSystem.DAL.DBContext;
using NewsManagementSystem.BusinessObject.Entities;

namespace NewsManagementSystem.DAL.Repositories.Category;

public class CategoryRepo : ICategoryRepo
{
    private readonly FUNewsManagementContext _context;

    public CategoryRepo(FUNewsManagementContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<List<BusinessObject.Entities.Category>> GetCategoriesAsync()
    {
        return await _context.Categories
            .Where(c => c.IsActive)
            .ToListAsync();
    }
    
    public async Task<BusinessObject.Entities.Category?> GetCategoryByNameAsync(string categoryName)
    {
        if (string.IsNullOrWhiteSpace(categoryName))
        {
            throw new ArgumentException("Category name cannot be null or empty.", nameof(categoryName));
        }

        return await _context.Categories
            .FirstOrDefaultAsync(c => c.CategoryName.ToLower() == categoryName.ToLower() && c.IsActive);
    }
    
    public async Task CreateCategoryAsync(BusinessObject.Entities.Category category)
    {
        if (category == null)
        {
            throw new ArgumentNullException(nameof(category));
        }

        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCategoryAsync(BusinessObject.Entities.Category category)
    {
        if (category == null)
        {
            throw new ArgumentNullException(nameof(category));
        }

        var existingCategory = await _context.Categories.FindAsync(category.CategoryId);
        if (existingCategory == null)
        {
            throw new KeyNotFoundException($"Category with ID {category.CategoryId} not found.");
        }
        //_context.Entry(existingCategory).CurrentValues.SetValues(category);
        //_context.Categories.Update(existingCategory);
        existingCategory.CategoryName = category.CategoryName;
        existingCategory.CategoryDesciption = category.CategoryDesciption;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCategoryAsync(BusinessObject.Entities.Category category)
    {
        if (category == null)
        {
            throw new ArgumentNullException(nameof(category));
        }

        var existingCategory = await _context.Categories.FindAsync(category.CategoryId);
        if (existingCategory == null)
        {
            throw new KeyNotFoundException($"Category with ID {category.CategoryId} not found.");
        }

        // Check if the category is associated with any NewsArticle
        var isCategoryInUse = await CategoryExistsAsync(category.CategoryId);
        if (isCategoryInUse)
        {
            //throw new InvalidOperationException("Cannot delete category because it is associated with one or more news articles.");
            return;
        }

        // Set IsActive to false instead of deleting the category
        existingCategory.IsActive = false;
        await _context.SaveChangesAsync();
    }

    public Task<bool> CategoryExistsAsync(short categoryId)
    {
        throw new NotImplementedException();
    }


    public async Task<BusinessObject.Entities.Category?> GetCategoryByIdAsync(int categoryId)
    {
        if (categoryId <= 0)
        {
            throw new ArgumentException("Category ID must be greater than zero.", nameof(categoryId));
        }

        return await _context.Categories.FindAsync(categoryId);
    }

    public async Task<bool> CategoryExistsAsync(int categoryId)
    {
        if (categoryId <= 0)
        {
            throw new ArgumentException("Category ID must be greater than zero.", nameof(categoryId));
        }

        // Check if the category exists in the NewsArticle table
        return await _context.NewsArticles.AnyAsync(na => na.CategoryId == categoryId);
    }

    public async Task<List<BusinessObject.Entities.Category>> SearchCategoriesByNameAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return await GetCategoriesAsync();
        }

        return await _context.Categories
            .Where(c => c.IsActive && c.CategoryName.ToLower().Contains(searchTerm.ToLower()))
            .ToListAsync();
    }
}