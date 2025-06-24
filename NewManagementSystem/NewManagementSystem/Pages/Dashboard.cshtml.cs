using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NewsManagementSystem.BLL.Services.Article;

namespace NewManagementSystem.Pages;

public class DashboardModel : PageModel
{
    private readonly IArticleService _articleService;

    public DashboardModel(IArticleService articleService)
    {
        _articleService = articleService;
    }

    [BindProperty(SupportsGet = true)] public DateTime? StartDate { get; set; }
    [BindProperty(SupportsGet = true)] public DateTime? EndDate { get; set; }

    public List<ChartData> ChartPoints { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        var from = StartDate?.Date ?? DateTime.Today.AddDays(-7);
        var to = (EndDate?.Date ?? DateTime.Today).AddDays(1).AddTicks(-1);

        var data = await _articleService.GetArticleByDateRange(from, to);


        ChartPoints = data
            .Where(a => a.CreatedDate.HasValue)
            .GroupBy(a => a.CreatedDate!.Value.Date)
            .Select(g => new ChartData
            {
                Date = g.Key.ToString("yyyy-MM-dd"),
                TotalArticles = g.Count()
            })
            .OrderByDescending(c => c.Date)
            .ToList();
        
        Console.WriteLine($"StartDate: {from:yyyy-MM-dd}, EndDate: {to:yyyy-MM-dd}");

        return Page();
    }

    public class ChartData
    {
        public string Date { get; set; }
        public int TotalArticles { get; set; }
    }
}
