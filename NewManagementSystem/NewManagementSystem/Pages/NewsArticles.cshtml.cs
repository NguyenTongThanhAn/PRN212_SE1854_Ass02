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

    public NewsArticlesModel(IArticleService articleService)
    {
        _articleService = articleService;
    }

    public async Task OnGetAsync()
    {
        Articles = await _articleService.GetActiveArticlesAsync();
    }
}
