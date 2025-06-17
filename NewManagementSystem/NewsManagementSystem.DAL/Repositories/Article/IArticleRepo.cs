using NewsManagementSystem.BusinessObject.Entities;

namespace NewsManagementSystem.DAL.Repositories.Article;

public interface IArticleRepo
{
    Task<List<NewsArticle>> GetArticleAsync();
    Task<List<NewsArticle>> GetArticlesyncOderByDescending();
    Task<List<NewsArticle>> GetActiveArticlesAsync();
    Task<List<NewsArticle>> GetArticlesByCategoryIdAsync(short categoryId);
    Task CreateArticleAsync(NewsArticle category);
    Task UpdateArticleAsync(NewsArticle category, List<int> selectedTagIds);
    Task DeleteArticleAsync(NewsArticle category);
    Task<List<NewsArticle>> GetArticlesByNameAsync(string search);
    Task<List<NewsArticle>> GetArticleByDateRange(DateTime? startDate, DateTime? endDate);
    Task<List<NewsArticle>> GetArticlesByAccountIdAsync(short userId);

}