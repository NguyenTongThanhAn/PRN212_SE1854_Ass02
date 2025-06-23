using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NewsManagementSystem.BLL.Services.Category;
using NewsManagementSystem.BusinessObject.Entities;
using System.ComponentModel.DataAnnotations;

namespace NewManagementSystem.Pages;

public class Categories : PageModel
{
    private readonly ICategoryService _categoryService;

    public List<Category> Category { get; set; } = new();

    [BindProperty]
    public Category NewCategory { get; set; } = new();

    [BindProperty]
    public Category EditCategory { get; set; } = new();

    [BindProperty]
    public int DeleteCategoryId { get; set; }

    public Dictionary<int, bool> CategoryHasArticle { get; set; } = new();

    public Categories(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task OnGet()
    {
        Category = await _categoryService.GetCategoriesAsync();
        CategoryHasArticle = new();
        foreach (var cat in Category)
        {
            CategoryHasArticle[cat.CategoryId] = await _categoryService.CategoryExistsAsync(cat.CategoryId);

        }
    }

    public async Task<IActionResult> OnPostCreateAsync()
    {
        if (!ModelState.IsValid)
        {
            await OnGet();
            return Page();
        }

        await _categoryService.CreateCategoryAsync(NewCategory);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostEditAsync()
    {
        if (!ModelState.IsValid)
        {
            await OnGet();
            return Page();
        }

        await _categoryService.UpdateCategoryAsync(EditCategory);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync()
    {
        var category = await _categoryService.GetCategoryByIdAsync(DeleteCategoryId);
        if (category != null)
        {
            await _categoryService.DeleteCategoryAsync(category);
        }
        return RedirectToPage();
    }

    public async Task<JsonResult> OnGetCategoryAsync(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        return new JsonResult(category);
    }
}
