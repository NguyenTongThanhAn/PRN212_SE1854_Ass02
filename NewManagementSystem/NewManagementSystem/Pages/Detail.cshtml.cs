using Microsoft.AspNetCore.Mvc.RazorPages;
using NewsManagementSystem.BLL.Services.Article;
using NewsManagementSystem.BusinessObject.Entities;

namespace NewManagementSystem.Pages;

public class DetailModel : PageModel
{
    private readonly IArticleService _articleService;
    
    public NewsArticle Article { get; set; }
    
    public DetailModel(IArticleService articleService)
    {
        _articleService = articleService;
    }
    
    public async Task OnGet(int id)
    { 
        Article = await _articleService.GetArticleByIdWithTagsAsync(id);
    }
}