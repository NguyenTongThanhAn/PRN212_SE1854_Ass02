using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using NewsManagementSystem.BLL.Services.SystemAccount;
using NewsManagementSystem.BusinessObject.Entities;
using System.Threading.Tasks;

namespace NewsManagementSystem.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ISystemAccountService _accountService;

        public LoginModel(ISystemAccountService accountService)
        {
            _accountService = accountService;
        }

        [BindProperty] public string Email { get; set; }
        [BindProperty] public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            // ✅ Kiểm tra dữ liệu thủ công
            if (string.IsNullOrWhiteSpace(Email))
            {
                ErrorMessage = "Email không được để trống.";
                return Page();
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Mật khẩu không được để trống.";
                return Page();
            }

            // ✅ Gọi service để xác thực
            var user = await _accountService.AuthenticateAsync(Email, Password);

            if (user == null)
            {
                ErrorMessage = "Email hoặc mật khẩu không đúng.";
                return Page();
            }

            // ✅ Lưu session sau khi đăng nhập
            HttpContext.Session.SetString("Role", user.AccountRole?.ToString() ?? "0");
            HttpContext.Session.SetString("Email", user.AccountEmail);
            HttpContext.Session.SetString("Name", user.AccountName);

            return RedirectToPage("/Articles");
        }
    }
}
