using NewsManagementSystem.BusinessObject.Entities;
using NewsManagementSystem.DAL.Repositories.Article;
using NewsManagementSystem.DAL.Repositories.Tag;

namespace NewsManagementSystem.BLL.Services.Article;

public class ArticleService : IArticleService
{
    private readonly IArticleRepo _articleRepo;
    private readonly ITagRepo _tagRepo;

    public ArticleService(IArticleRepo articleRepo, ITagRepo tagRepo)
    {
        _articleRepo = articleRepo;
        _tagRepo = tagRepo;
    }

    public Task<List<NewsArticle>> GetArticleAsync() => _articleRepo.GetArticleAsync();
    public async Task<List<NewsArticle>> GetActiveArticlesAsync()
    {
        return await _articleRepo.GetActiveArticlesAsync();
    }


    public async Task<List<NewsArticle>> GetArticleByNameAsync(string search) => await _articleRepo.GetArticlesByNameAsync(search);

    public async Task<NewsArticle?> GetArticleByIdWithTagsAsync(int id)
    {
        var list = await _articleRepo.GetArticleAsync();
        return list.FirstOrDefault(x => x.NewsArticleId == id);
    }
    
    public async Task<List<NewsArticle>> GetArticlesByCategoryIdAsync(short categoryId)
    {
        return await _articleRepo.GetArticlesByCategoryIdAsync(categoryId);
    }

    public Task CreateArticleAsync(NewsArticle article) => _articleRepo.CreateArticleAsync(article);

    public async Task CreateArticleWithTagsAsync(NewsArticle article, List<int> tagIds)
    {
        var tags = await _tagRepo.GetTagsByIdsAsync(tagIds);
        article.Tags = tags;
        await _articleRepo.CreateArticleAsync(article);
    }
    
    public async Task UpdateArticleWithTagsAsync(NewsArticle article, List<int> tagIds)
    {
        await _articleRepo.UpdateArticleAsync(article, tagIds);
    }


    public Task DeleteArticleAsync(NewsArticle article) => _articleRepo.DeleteArticleAsync(article);

    public async Task DeleteArticleByIdAsync(int id)
    {
        var article = await GetArticleByIdWithTagsAsync(id);
        if (article != null)
        {
            await _articleRepo.DeleteArticleAsync(article);
        }
    }

    public async Task<List<NewsArticle>> GetArticlesyncOrderByDesending()
    {
        return await _articleRepo.GetArticlesyncOderByDescending();
    }

    public async Task<List<NewsArticle>> GetArticleByDateRange(DateTime? startDate, DateTime? endDate)
    {
        return await _articleRepo.GetArticleByDateRange(startDate, endDate);
    }
    
    public async Task<List<NewsArticle>> GetArticlesByAccountIdAsync(short userId)
    {
        return  await _articleRepo.GetArticlesByAccountIdAsync(userId);
    }
    
    
}
