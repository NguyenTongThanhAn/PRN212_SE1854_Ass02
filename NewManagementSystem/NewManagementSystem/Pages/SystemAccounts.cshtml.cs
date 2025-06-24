using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NewManagementSystem.ViewModel;
using NewsManagementSystem.BLL.Services.SystemAccount;
using NewsManagementSystem.BusinessObject.Entities;

namespace NewManagementSystem.Pages;

public class SystemAccountsModel : PageModel
{
    private readonly ISystemAccountService _service;

    [BindProperty] public CreateSystemAccountRequest NewAccount { get; set; }
    [BindProperty] public EditSystemAccountRequest EditAccount { get; set; }
    [BindProperty] public int DeleteAccountId { get; set; }

    public List<SystemAccount> Accounts { get; set; } = new();
    public string SearchKeyword { get; set; }

    public SystemAccountsModel(ISystemAccountService service)
    {
        _service = service;
    }

    public async Task<IActionResult> OnGetAsync(string? keyword)
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != "0") return RedirectToPage("/AccessDenied");

        SearchKeyword = keyword;
        Accounts = string.IsNullOrWhiteSpace(keyword)
            ? await _service.GetSystemAccountsAsync()
            : await _service.SearchAsync(keyword);

        ViewData["ShowCreateModal"] = "False";
        ViewData["ShowEditModal"] = "False";
        ViewData["ShowDeleteModal"] = "False";

        return Page();
    }

    public async Task<IActionResult> OnPostCreateAsync()
    {
        var account = new SystemAccount
        {
            AccountName = NewAccount.AccountName,
            AccountEmail = NewAccount.AccountEmail,
            AccountRole = NewAccount.AccountRole,
            AccountPassword = NewAccount.AccountPassword,
        };
        await _service.CreateSystemAccountAsync(account);
        return RedirectToPage("./SystemAccounts");
    }

    public async Task<IActionResult> OnPostEditAsync()
    {
        var account = new SystemAccount
        {
            AccountId = EditAccount.AccountId,
            AccountName = EditAccount.AccountName,
            AccountEmail = EditAccount.AccountEmail,
            AccountRole = EditAccount.AccountRole,
        };
        await _service.UpdateSystemAccountAsync(account);
        return RedirectToPage("./SystemAccounts");
    }

    public async Task<IActionResult> OnPostDeleteAsync()
    {
        var account = await _service.GetSystemAccountByIdAsync((short)DeleteAccountId);
        if (account != null)
        {
            await _service.DeleteSystemAccountAsync(account);
        }
        return RedirectToPage("./SystemAccounts");
    }
}
