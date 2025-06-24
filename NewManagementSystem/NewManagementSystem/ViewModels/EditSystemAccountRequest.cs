namespace NewManagementSystem.ViewModel;

public class EditSystemAccountRequest
{
    public int AccountId { get; set; }
    public string AccountName { get; set; }
    public string AccountEmail { get; set; }
    public string AccountPassword { get; set; }
    public int AccountRole { get; set; }
}