using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NewsManagementSystem.BLL.Services.Category;
using NewsManagementSystem.BusinessObject.Entities;

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

    public async Task<IActionResult> OnGetAsync()
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != "1") // ❌ Nếu không phải Staff
        {
            return RedirectToPage("/AccessDenied");
        }

        Category = await _categoryService.GetCategoriesAsync();
        CategoryHasArticle = new();

        foreach (var cat in Category)
        {
            CategoryHasArticle[cat.CategoryId] = await _categoryService.CategoryExistsAsync(cat.CategoryId);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostCreateAsync()
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != "1")
        {
            return RedirectToPage("/AccessDenied");
        }

        if (!ModelState.IsValid)
        {
            await OnGetAsync();
            return Page();
        }

        await _categoryService.CreateCategoryAsync(NewCategory);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostEditAsync()
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != "1")
        {
            return RedirectToPage("/AccessDenied");
        }

        if (!ModelState.IsValid)
        {
            await OnGetAsync();
            return Page();
        }

        await _categoryService.UpdateCategoryAsync(EditCategory);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync()
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != "1")
        {
            return RedirectToPage("/AccessDenied");
        }

        var category = await _categoryService.GetCategoryByIdAsync(DeleteCategoryId);
        if (category != null)
        {
            await _categoryService.DeleteCategoryAsync(category);
        }
        return RedirectToPage();
    }

    public async Task<JsonResult> OnGetCategoryAsync(int id)
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != "1")
        {
            return new JsonResult(new { error = "Access Denied" });
        }

        var category = await _categoryService.GetCategoryByIdAsync(id);
        return new JsonResult(category);
    }
}
