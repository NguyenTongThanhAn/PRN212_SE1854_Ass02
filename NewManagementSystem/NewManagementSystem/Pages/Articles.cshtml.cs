using Microsoft.AspNetCore.Mvc.RazorPages;
using NewManagementSystem.ViewModel;
using NewsManagementSystem.BLL.Services.Article;
using NewsManagementSystem.BusinessObject.Entities;

namespace NewManagementSystem.Pages;

public class ArticlesModel : PageModel
{
    private readonly IArticleService _articleService;

    public List<NewsArticle> Articles { get; set; } = new();

    public ArticlesModel(IArticleService articleService)
    {
        _articleService = articleService;
    }

    public async Task OnGet()
    { 
        Articles = await _articleService.GetActiveArticlesAsync();
    }
}
