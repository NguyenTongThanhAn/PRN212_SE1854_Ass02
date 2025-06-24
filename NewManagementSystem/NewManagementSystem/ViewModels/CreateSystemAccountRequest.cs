namespace NewManagementSystem.ViewModel;

public class CreateSystemAccountRequest
{
    public string AccountName { get; set; }
    public string AccountEmail { get; set; }
    public int AccountRole { get; set; }
    public string AccountPassword { get; set; }
}