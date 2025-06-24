﻿using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using NewManagementSystem.SignalR.Hubs;
using NewManagementSystem.Validation;
using NewManagementSystem.ViewModel;
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
    
    private readonly IValidator<CreateArticlesRequest> _createArticleReqValidator;
    private readonly IValidator<EditArticlesRequest> _editArticleReqValidator;
    
    private readonly IHubContext<ArticlesHub> _hubContext;
    
    [BindProperty]
    public CreateArticlesRequest ArticlesRequest { get; set; }
    [BindProperty]
    public EditArticlesRequest EditArticlesRequest { get; set; }
    [BindProperty]
    public int ArticleIdToDelete { get; set; }

    public List<NewsArticle> Articles { get; set; } = new();
    public List<Tag> Tags { get; set; } = new();
    public List<Category> Categories { get; set; } = new();

    public ArticlesModel(IArticleService articleService, ITagService tagService, ICategoryService categoryService, IValidator<CreateArticlesRequest> createArticleReqValidator, IValidator<EditArticlesRequest> editArticleReqValidator, IHubContext<ArticlesHub> hubContext)
    {
        _articleService = articleService;
        _tagService = tagService;
        _categoryService = categoryService;
        _createArticleReqValidator = createArticleReqValidator;
        _editArticleReqValidator = editArticleReqValidator;
        _hubContext = hubContext;
    }

    public async Task<IActionResult> OnGet()
    { 
        var role = HttpContext.Session.GetString("Role");
        var userId = HttpContext.Session.GetInt32("UserId");
        if (role != "1" && role != "0")
            return RedirectToPage("/AccessDenied");
        Articles = await _articleService.GetArticleAsync();
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
        var userId = HttpContext.Session.GetInt32("UserId");
        if (role != "1" && role != "0")
            return RedirectToPage("/AccessDenied");
        
        ValidationResult result = await _createArticleReqValidator.ValidateAsync(ArticlesRequest);
        Articles = await _articleService.GetArticleAsync();
        Tags = await _tagService.GetAllTagsAsync();
        Categories = await _categoryService.GetCategoriesAsync();
        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError($"ArticlesRequest.{error.PropertyName}", error.ErrorMessage);
            }
            ViewData["ShowCreateModal"] = "True";
            return Page();
        }

// Sau khi tạo thành công
        var article = new NewsArticle()
        {
            NewsTitle = ArticlesRequest.NewsTitle,
            Headline = ArticlesRequest.Headline,
            NewsContent = ArticlesRequest.NewsContent,
            NewsSource = ArticlesRequest.NewsSource,
            CategoryId = ArticlesRequest.CategoryId,
            NewsStatus = ArticlesRequest.NewsStatus,
            CreatedById = userId,    //Sẽ thay bằng ID sau khi login
            Tags = (Tags.Where(t => ArticlesRequest.Tags.Contains(t.TagId))).ToList()
        };

        var created = await _articleService.CreateArticleAsync(article);
        var articleDto = new
        {
            created.NewsArticleId,
            created.NewsTitle,
            created.Headline,
            created.NewsContent,
            created.NewsSource,
            CategoryName = created.Category != null ? created.Category.CategoryName : null,
            created.NewsStatus,
            created.CreatedBy.AccountName,
            created.CreatedDate,
            created.ModifiedDate,
            Tags = created.Tags?.Select(t => new
            {
                t.TagId,
                t.TagName
            }).ToList()
        };
        await _hubContext.Clients.All.SendAsync("ReceiveNewArticle", articleDto);
        // Load lại danh sách bài viết mới nhất
        return RedirectToPage("./Articles");

    }
    
    public async Task<IActionResult> OnPostEditAsync()
    {
        var role = HttpContext.Session.GetString("Role");
        var userId = HttpContext.Session.GetInt32("UserId");
        if (role != "1" && role != "0")
            return RedirectToPage("/AccessDenied");
        
        if (EditArticlesRequest.CreatedById != userId)
            return RedirectToPage("/AccessDenied");
        
        ValidationResult result = await _editArticleReqValidator.ValidateAsync(EditArticlesRequest);
        Articles = await _articleService.GetArticleAsync();
        Tags = await _tagService.GetAllTagsAsync();
        Categories = await _categoryService.GetCategoriesAsync();
        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError($"EditArticlesRequest.{error.PropertyName}", error.ErrorMessage);
            }
            ViewData["ShowEditModal"] = "True";
            return Page();
        }

// Sau khi tạo thành công
        var article = new NewsArticle()
        {
            NewsArticleId = EditArticlesRequest.NewsArticleId,
            NewsTitle = EditArticlesRequest.NewsTitle,
            Headline = EditArticlesRequest.Headline,
            NewsContent = EditArticlesRequest.NewsContent,
            NewsSource = EditArticlesRequest.NewsSource,
            CategoryId = EditArticlesRequest.CategoryId,
            NewsStatus = EditArticlesRequest.NewsStatus,
            UpdatedById = userId,    //sẽ thay bằng ID sau khi login
            Tags = (Tags.Where(t => EditArticlesRequest.Tags.Contains(t.TagId))).ToList()
        };

        var updated = await _articleService.UpdateArticleAsync(article);
        var articleDto = new {
            updated.NewsArticleId,
            updated.NewsTitle,
            updated.Headline,
            updated.NewsContent,
            updated.NewsSource,
            CategoryName = updated.Category != null ? updated.Category.CategoryName : null,
            updated.NewsStatus,
            updated.CreatedBy.AccountName,
            updated.CreatedDate,
            updated.ModifiedDate,
            Tags = updated.Tags?.Select(t => new {
                t.TagId,
                t.TagName
            }).ToList()
            // Các trường đơn giản bạn muốn gửi
        };
        try
        {
            await _hubContext.Clients.All.SendAsync("ReceiveUpdatedArticle", articleDto);
        }
        catch (Exception ex)
        {
            Console.WriteLine("SignalR SendAsync Error: " + ex.Message);
            // Có thể log chi tiết hơn ex.StackTrace
        }

        // Load lại danh sách bài viết mới nhất
        return RedirectToPage("./Articles");
    }

    public async Task<IActionResult> OnPostDeleteAsync()
    {
        var role = HttpContext.Session.GetString("Role");
        var userId = HttpContext.Session.GetInt32("UserId");
        if (role != "1" && role != "0")
            return RedirectToPage("/AccessDenied");
        
        Articles = await _articleService.GetArticleAsync();
        Tags = await _tagService.GetAllTagsAsync();
        Categories = await _categoryService.GetCategoriesAsync();
        var deleted = await _articleService.DeleteArticleByIdAsync(ArticleIdToDelete);
        var articleDto = new {
            deleted.NewsArticleId,
            deleted.NewsTitle,
            deleted.Headline,
            deleted.NewsContent,
            deleted.NewsSource,
            CategoryName = deleted.Category != null ? deleted.Category.CategoryName : null,
            deleted.NewsStatus,
            deleted.CreatedBy.AccountName,
            deleted.CreatedDate,
            deleted.ModifiedDate,
            Tags = deleted.Tags?.Select(t => new {
                t.TagId,
                t.TagName
            }).ToList()
            // Các trường đơn giản bạn muốn gửi
        };
        await _hubContext.Clients.All.SendAsync("ReceiveDeletedArticle", articleDto);
        return RedirectToPage("./Articles");
    }

    // public async Task<IActionResult> OnPostConfirmDeleteAsync()
    // {
    //     await _articleService.DeleteArticleByIdAsync(ArticleIdToDelete);
    //     return RedirectToPage("./Articles");
    // }
}