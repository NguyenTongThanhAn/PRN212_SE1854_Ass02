using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using NewManagementSystem.SignalR.Hubs;
using NewsManagementSystem.BLL.Services.Article;
using NewsManagementSystem.BLL.Services.Category;
using NewsManagementSystem.BLL.Services.Tag;
using NewsManagementSystem.BusinessObject.Entities;

namespace NewManagementSystem.Pages;

public class ArticlesModel : PageModel
{
    private readonly IArticleService _articleService;
    private readonly ITagService _tagService;
    private readonly ICategoryService _categoryService;
    private readonly IHubContext<ArticlesHub> _hubContext;

    [BindProperty] public NewsArticle ArticlesRequest { get; set; }
    [BindProperty] public NewsArticle EditArticlesRequest { get; set; }
    [BindProperty] public int ArticleIdToDelete { get; set; }

    public List<NewsArticle> Articles { get; set; } = new();
    public List<Tag> Tags { get; set; } = new();
    public List<Category> Categories { get; set; } = new();

    public ArticlesModel(
        IArticleService articleService,
        ITagService tagService,
        ICategoryService categoryService,
        IHubContext<ArticlesHub> hubContext)
    {
        _articleService = articleService;
        _tagService = tagService;
        _categoryService = categoryService;
        _hubContext = hubContext;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var role = HttpContext.Session.GetString("Role");

        if (string.IsNullOrEmpty(role)) // ❌ Guest không được truy cập trang này
        {
            return RedirectToPage("/AccessDenied");
        }

        if (role == "2") // Lecturer chỉ được xem bài active
        {
            Articles = await _articleService.GetActiveArticlesAsync();
        }
        else
        {
            Articles = await _articleService.GetArticleAsync();
        }

        Tags = await _tagService.GetAllTagsAsync();
        Categories = await _categoryService.GetCategoriesAsync();
        ViewData["ShowCreateModal"] = "False";
        ViewData["ShowEditModal"] = "False";
        ViewData["ShowDeleteModal"] = "False";

        return Page();
    }

    public async Task<IActionResult> OnPostCreateAsync()
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != "1" && role != "0") // Not Staff or Admin
            return RedirectToPage("/AccessDenied");

        Tags = await _tagService.GetAllTagsAsync();
        Categories = await _categoryService.GetCategoriesAsync();

        var article = new NewsArticle()
        {
            NewsTitle = ArticlesRequest.NewsTitle,
            Headline = ArticlesRequest.Headline,
            NewsContent = ArticlesRequest.NewsContent,
            NewsSource = ArticlesRequest.NewsSource,
            CategoryId = ArticlesRequest.CategoryId,
            NewsStatus = ArticlesRequest.NewsStatus,
            CreatedById = 1, // TODO: lấy ID người dùng từ session
            Tags = Tags.Where(t => ArticlesRequest.Tags.Any(tag => tag.TagId == t.TagId)).ToList()
        };

        var created = await _articleService.CreateArticleAsync(article);
        await _hubContext.Clients.All.SendAsync("ReceiveNewArticle", created);
        return RedirectToPage("./Articles");
    }

    public async Task<IActionResult> OnPostEditAsync()
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != "1" && role != "0") // Not Staff or Admin
            return RedirectToPage("/AccessDenied");

        Tags = await _tagService.GetAllTagsAsync();
        Categories = await _categoryService.GetCategoriesAsync();

        var article = new NewsArticle()
        {
            NewsArticleId = EditArticlesRequest.NewsArticleId,
            NewsTitle = EditArticlesRequest.NewsTitle,
            Headline = EditArticlesRequest.Headline,
            NewsContent = EditArticlesRequest.NewsContent,
            NewsSource = EditArticlesRequest.NewsSource,
            CategoryId = EditArticlesRequest.CategoryId,
            NewsStatus = EditArticlesRequest.NewsStatus,
            UpdatedById = 1, // TODO: lấy ID thực tế
            Tags = Tags.Where(t => EditArticlesRequest.Tags.Any(tag => tag.TagId == t.TagId)).ToList()
        };

        var updated = await _articleService.UpdateArticleAsync(article);
        await _hubContext.Clients.All.SendAsync("ReceiveUpdatedArticle", updated);
        return RedirectToPage("./Articles");
    }

    public async Task<IActionResult> OnPostDeleteAsync()
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != "1" && role != "0")
            return RedirectToPage("/AccessDenied");

        Tags = await _tagService.GetAllTagsAsync();
        Categories = await _categoryService.GetCategoriesAsync();

        var deleted = await _articleService.DeleteArticleByIdAsync(ArticleIdToDelete);
        await _hubContext.Clients.All.SendAsync("ReceiveDeletedArticle", deleted);
        return RedirectToPage("./Articles");
    }
}
