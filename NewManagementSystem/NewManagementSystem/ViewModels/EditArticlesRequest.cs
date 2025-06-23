namespace NewManagementSystem.ViewModel;

public class EditArticlesRequest
{
    public int NewsArticleId { get; set; }
    public string NewsTitle { get; set; }
    
    public string Headline { get; set; }
    
    public string NewsContent { get; set; }

    public string NewsSource { get; set; }

    public int CategoryId { get; set; }
    
    public bool NewsStatus { get; set; }
    
    public int CreatedById { get; set; }
    
    public string? ImageUrl { get; set; }
    
    public List<int> Tags { get; set; } = new();
}