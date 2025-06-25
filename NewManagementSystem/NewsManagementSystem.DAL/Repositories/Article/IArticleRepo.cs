using NewsManagementSystem.BusinessObject.Entities;

namespace NewsManagementSystem.DAL.Repositories.Article;

public interface IArticleRepo
{
    Task<List<NewsArticle>> GetArticleAsync();
    Task<List<NewsArticle>> GetArticlesyncOderByDescending();
    Task<NewsArticle?> GetArticleByIdAsync(int articleId);
    Task<List<NewsArticle>> GetActiveArticlesAsync();
    Task<List<NewsArticle>> GetArticlesByCategoryIdAsync(short categoryId);
    Task<NewsArticle> CreateArticleAsync(NewsArticle category);
    Task<NewsArticle> UpdateArticleAsync(NewsArticle category);
    Task<NewsArticle> DeleteArticleAsync(int articleID);
    Task<List<NewsArticle>> GetArticlesByNameAsync(string search);
    Task<List<NewsArticle>> GetArticleByDateRange(DateTime? startDate, DateTime? endDate);
    Task<List<NewsArticle>> GetArticlesByAccountIdAsync(int userId);

}