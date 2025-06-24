using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NewManagementSystem.SignalR.Hubs;
using NewManagementSystem.Validation;
using NewManagementSystem.ViewModel;
using NewsManagementSystem.BLL.Services.Article;
using NewsManagementSystem.BLL.Services.Category;
using NewsManagementSystem.BLL.Services.SystemAccount;
using NewsManagementSystem.BLL.Services.Tag;
using NewsManagementSystem.DAL.DBContext;
using NewsManagementSystem.DAL.Repositories.Article;
using NewsManagementSystem.DAL.Repositories.Category;
using NewsManagementSystem.DAL.Repositories.SystemAccount;
using NewsManagementSystem.DAL.Repositories.Tag;
using NewsManagementSystem.DAL.SystemAccount;

namespace NewsManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 🔧 Register DB Context
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            Console.WriteLine($"Connection string: {connectionString}");
            builder.Services.AddDbContext<FUNewsManagementContext>(options =>
                options.UseSqlServer(connectionString));

            // 🔧 Register repositories
            builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();
            builder.Services.AddScoped<IArticleRepo, ArticleRepo>();
            builder.Services.AddScoped<ITagRepo, TagRepo>();
            builder.Services.AddScoped<ISystemAccountRepo, SystemAccountRepo>();

            // 🔧 Register services
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IArticleService, ArticleService>();
            builder.Services.AddScoped<ITagService, TagService>();
            builder.Services.AddScoped<ISystemAccountService, SystemAccountService>();

            // ✅ Register Session
            builder.Services.AddSession();

            // ✅ Optional: If you need to inject IHttpContextAccessor
            builder.Services.AddHttpContextAccessor();

            // ✅ Add Razor Pages
            builder.Services.AddRazorPages();
            builder.Services.AddSignalR();

            var app = builder.Build();

            // ✅ Configure middleware pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.MapHub<ArticlesHub>("/articlesHub");

            app.UseRouting();

            // ✅ Enable Session before authorization
            app.UseSession();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
