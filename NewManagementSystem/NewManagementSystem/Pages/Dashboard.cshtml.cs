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

    public List<ChartData> ChartPointsByDay { get; set; } = new();
    public List<ChartData> ChartPointsByYear { get; set; } = new();
    public bool ShowChart { get; set; }
    public async Task<IActionResult> OnGetAsync()
    {
        ShowChart = StartDate.HasValue && EndDate.HasValue;

        if (ShowChart)
        {
            var from = StartDate.Value.Date;
            var to = EndDate.Value.Date.AddDays(1).AddTicks(-1);

            var data = await _articleService.GetArticleByDateRange(from, to);

            // Theo ngày
            ChartPointsByDay = data
                .Where(a => a.CreatedDate.HasValue)
                .GroupBy(a => a.CreatedDate!.Value.Date)
                .Select(g => new ChartData
                {
                    Date = g.Key.ToString("yyyy-MM-dd"),
                    TotalArticles = g.Count()
                })
                .OrderByDescending(c => c.Date)
                .ToList();

            // Theo năm
            ChartPointsByYear = data
                .Where(a => a.CreatedDate.HasValue)
                .GroupBy(a => a.CreatedDate!.Value.Year)
                .Select(g => new ChartData
                {
                    Date = g.Key.ToString(), // Năm
                    TotalArticles = g.Count()
                })
                .OrderByDescending(c => c.Date)
                .ToList();
        }
        else
        {
            ChartPointsByDay = new List<ChartData>();
            ChartPointsByYear = new List<ChartData>();
        }

        return Page();
    }

    public class ChartData
    {
        public string Date { get; set; } = string.Empty;
        public int TotalArticles { get; set; }
    }
}
