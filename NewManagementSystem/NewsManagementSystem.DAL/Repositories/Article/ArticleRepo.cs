using NewsManagementSystem.BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;
using NewsManagementSystem.DAL.DBContext;

namespace NewsManagementSystem.DAL.Repositories.Article;

public class ArticleRepo : IArticleRepo
{
    private readonly FUNewsManagementContext _context;

    public ArticleRepo(FUNewsManagementContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<List<NewsArticle>> GetArticleAsync()
    {
        return await _context.NewsArticles
            .Include(a => a.Category)
            .Include(a => a.Tags)
            .Include(a => a.CreatedBy)
            .OrderByDescending(a => a.CreatedDate)
            .ToListAsync();
    }

    public async Task<NewsArticle?> GetArticleByNameAsync(string name)
    {
        return await _context.NewsArticles
            .Include(a => a.Tags)
            .FirstOrDefaultAsync(a => a.NewsTitle == name);
    }

    public async Task<NewsArticle?> GetArticleByIdAsync(int articleId)
    {
        return await _context.NewsArticles
            .Include(a => a.Category)
            .Include(a => a.Tags)
            .Include(a => a.CreatedBy)
            .FirstOrDefaultAsync(a => a.NewsArticleId == articleId);
    }

    public async Task<List<NewsArticle>> GetActiveArticlesAsync()
    {
        return await _context.NewsArticles
            .Where(a => (bool)a.NewsStatus == true)
            .Include(a => a.Category)
            .Include(a => a.Tags)
            .Include(a => a.CreatedBy)
            .OrderByDescending(a => a.ModifiedDate)
            .ThenBy(a => a.CreatedDate)
            .ToListAsync();
    }


    public async Task<List<NewsArticle>> GetArticlesByCategoryIdAsync(short categoryId)
    {
        return await _context.NewsArticles
            .Where(a => a.CategoryId == categoryId)
            .Include(a => a.Tags)
            .ToListAsync();
    }


    public async Task<NewsArticle> CreateArticleAsync(NewsArticle article)
    {
        if (article.Tags != null && article.Tags.Any())
        {
            foreach (var tag in article.Tags)
            {
                _context.Entry(tag).State = EntityState.Unchanged;
            }
        }
        article.CreatedDate = DateTime.UtcNow;
        _context.NewsArticles.Add(article);
        await _context.SaveChangesAsync();
        var created = await _context.NewsArticles
            .Include(a => a.Tags)
            .Include(a => a.Category)
            .Include(a => a.CreatedBy)
            .FirstOrDefaultAsync(a => a.NewsArticleId == article.NewsArticleId);
        return created;
    }

    public async Task<NewsArticle> UpdateArticleAsync(NewsArticle article)
    {
        var result = await _context.NewsArticles
            .Include(a => a.Tags)
            .FirstOrDefaultAsync(x => x.NewsArticleId == article.NewsArticleId);

        if (result == null) return null;

        if (!string.Equals(result.NewsTitle, article.NewsTitle))
            result.NewsTitle = article.NewsTitle;
        if (!string.Equals(result.Headline, article.Headline))
            result.Headline = article.Headline;
        if (!string.Equals(result.NewsContent, article.NewsContent))
            result.NewsContent = article.NewsContent;
        if (!string.Equals(result.NewsSource, article.NewsSource)) 
            result.NewsSource = article.NewsSource;
        if (result.NewsStatus != article.NewsStatus)
            result.NewsStatus = article.NewsStatus;
        if (result.CategoryId != article.CategoryId)
            result.CategoryId = article.CategoryId;
        result.ModifiedDate = DateTime.UtcNow;
        result.Tags?.Clear();
        if (article.Tags != null && article.Tags.Count > 0)
        {
            var tags = await _context.Tags.Where(t => article.Tags.Select(tag => tag.TagId).Contains(t.TagId)).ToListAsync();
            foreach (var tag in tags)
            {
                result.Tags.Add(tag);
            }
        }

        await _context.SaveChangesAsync();
        return result;
    }



    public async Task<NewsArticle> DeleteArticleAsync(int newsArticleId)
    {
        var result = await _context.NewsArticles
            .Include(a => a.Tags)
            .FirstOrDefaultAsync(x => x.NewsArticleId == newsArticleId);
            
        if (result == null) return null;
            
        result.NewsStatus = false;
        await _context.SaveChangesAsync();
        return result;
    }

    public async Task<List<NewsArticle>> GetArticlesByNameAsync(string search)
    {
        return await _context.NewsArticles.Where(a => (a.NewsTitle.ToLower().Contains(search.ToLower())) && (a.NewsStatus == true)).OrderByDescending(a => a.NewsArticleId).Include(a => a.Tags).ToListAsync();
    }

    public async Task<List<NewsArticle>> GetArticlesyncOderByDescending()
    {
        var result = await _context.NewsArticles
            .Include(a => a.Tags).Include(a=>a.Category)
            .OrderByDescending(a => a.CreatedDate)
            .ToListAsync();
        return result;
    }

    public async Task<List<NewsArticle>> GetArticleByDateRange(DateTime? startDate, DateTime? endDate)
    {
        var fromDate = startDate?.Date ?? DateTime.MinValue.Date;
        var toDate = endDate?.Date ?? DateTime.MaxValue.Date;

        var allArticles = await _context.NewsArticles
            .Include(a => a.Tags)
            .Include(a => a.Category)
            .Where(a => a.CreatedDate != null)
            .ToListAsync();

        return await _context.NewsArticles
    .Where(a => a.CreatedDate >= startDate && a.CreatedDate <= endDate)
    .ToListAsync();
    }


    
    public async Task<List<NewsArticle>> GetArticlesByAccountIdAsync(int userId)
    {
        return await _context.NewsArticles
            .Where(x => x.CreatedById == userId)
            .Include(x => x.CreatedBy)
            .ToListAsync();
    }
    
}