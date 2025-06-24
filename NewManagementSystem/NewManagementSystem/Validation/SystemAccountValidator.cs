using FluentValidation;
using NewsManagementSystem.BusinessObject.Entities;

namespace NewManagementSystem.Validation
{
    public class SystemAccountValidator : AbstractValidator<SystemAccount>
    {
        public SystemAccountValidator()
        {
            RuleFor(x => x.AccountEmail)
                .NotEmpty().WithMessage("Email không được để trống")
                .EmailAddress().WithMessage("Email không hợp lệ");

            RuleFor(x => x.AccountPassword)
                .NotEmpty().WithMessage("Mật khẩu không được để trống")
                .MinimumLength(6).WithMessage("Mật khẩu phải từ 6 ký tự");
        }
    }
}
