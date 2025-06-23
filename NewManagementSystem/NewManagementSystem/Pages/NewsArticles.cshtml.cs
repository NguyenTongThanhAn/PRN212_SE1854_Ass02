using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NewsManagementSystem.BLL.Services.Article;
using NewsManagementSystem.BusinessObject.Entities;

namespace NewManagementSystem.Pages;

public class NewsArticlesModel : PageModel
{
    private readonly IArticleService _articleService;
    
    [BindProperty]
    public List<NewsArticle> Articles { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; } = 5; 
    
    public NewsArticlesModel(IArticleService articleService)
    {
        _articleService = articleService;
    }
    
    public async Task OnGetAsync([FromQuery]int page)
    {
        page = page < 1 ? 1 : page;
        var allArticles = await _articleService.GetActiveArticlesAsync(); // or query from DB directly

        int totalArticles = allArticles.Count;
        TotalPages = (int)Math.Ceiling(totalArticles / (double)PageSize);
        CurrentPage = page;

        Articles = allArticles
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .ToList();
    }
}