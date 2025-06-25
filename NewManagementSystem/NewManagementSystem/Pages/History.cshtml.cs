using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NewsManagementSystem.BLL.Services.Article;
using NewsManagementSystem.BLL.Services.Category;
using NewsManagementSystem.BusinessObject.Entities;

namespace NewsManagementSystem;

public class HistoryModel : PageModel
{
    private readonly IArticleService _articleService;
    private readonly ICategoryService _categoryService;

    public List<NewsArticle> Articles { get; set; }
    
    public List<Category> Categories { get; set; }

    public HistoryModel(IArticleService articleService, ICategoryService categoryService)
    {
        _articleService = articleService;
        _categoryService = categoryService;
    }
    
    public async Task<IActionResult> OnGet(int id)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
            return RedirectToPage("/AccessDenied");
        Articles = await _articleService.GetArticlesByAccountIdAsync(id);
        Categories = await _categoryService.GetCategoriesActiveAsync();
        return Page();
    }
}