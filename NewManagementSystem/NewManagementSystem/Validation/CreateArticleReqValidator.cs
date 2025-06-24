using FluentValidation;
using NewManagementSystem.ViewModel;

namespace NewManagementSystem.Validation;

public class CreateArticleReqValidator : AbstractValidator<CreateArticlesRequest>
{
    public CreateArticleReqValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Continue;
        
        RuleFor(a => a.NewsTitle)
            .NotEmpty().WithMessage("News Title is required")
            .Length(5, 200).WithMessage("News Title must be between 5 and 200 characters");
        RuleFor(a => a.Headline)
            .NotEmpty().WithMessage("Headline is required")
            .Length(5, 300).WithMessage("Headline must be between 5 and 300 characters");
        RuleFor(a => a.NewsContent)
            .NotEmpty().WithMessage("News Content is required")
            .Length(5, 4000).WithMessage("News Content must be between 5 and 4000 characters");
        RuleFor(a => a.NewsSource)
            .NotEmpty().WithMessage("News Source is required")
            .Length(5, 400).WithMessage("News Source must be between 5 and 400 characters");
        RuleFor(a => a.NewsStatus)
            .NotNull().WithMessage("News Status is required");
        RuleFor(a => a.CategoryId)
            .NotEmpty().WithMessage("Category is required");
        RuleFor(a => a.Tags)
            .NotEmpty().WithMessage("Tags are required")
            .Must(a => a.Count > 0).WithMessage("Tags are required");
            
    }
}