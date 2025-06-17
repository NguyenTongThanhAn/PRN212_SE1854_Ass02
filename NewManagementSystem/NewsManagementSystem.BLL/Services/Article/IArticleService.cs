using NewsManagementSystem.BusinessObject.Entities;

namespace NewsManagementSystem.BLL.Services.Article;

public interface IArticleService
{
    Task<List<NewsArticle>> GetArticleAsync();
    Task<List<NewsArticle>> GetArticlesyncOrderByDesending();
    Task<List<NewsArticle>> GetActiveArticlesAsync();
    Task<List<NewsArticle>> GetArticleByNameAsync(string name);
    Task<NewsArticle?> GetArticleByIdWithTagsAsync(int id);
    Task<List<NewsArticle>> GetArticlesByCategoryIdAsync(short categoryId);
    Task CreateArticleAsync(NewsArticle article);
    Task CreateArticleWithTagsAsync(NewsArticle article, List<int> tagIds);
    Task UpdateArticleWithTagsAsync(NewsArticle article, List<int> tagIds);
    Task DeleteArticleAsync(NewsArticle article);
    Task DeleteArticleByIdAsync(int id);
    Task<List<NewsArticle>> GetArticleByDateRange(DateTime? startDate, DateTime? endDate);
    Task<List<NewsArticle>> GetArticlesByAccountIdAsync(short userId);
    
    
}