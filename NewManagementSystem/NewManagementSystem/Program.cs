using Microsoft.EntityFrameworkCore;
using NewsManagementSystem.BLL.Services.Article;
using NewsManagementSystem.BLL.Services.Category;
using NewsManagementSystem.BLL.Services.SystemAccount;
using NewsManagementSystem.BLL.Services.Tag;
using NewsManagementSystem.DAL.DBContext;
using NewsManagementSystem.DAL.Repositories.Article;
using NewsManagementSystem.DAL.Repositories.Category;
using NewsManagementSystem.DAL.Repositories.Tag;
using NewsManagementSystem.DAL.SystemAccount;

namespace NewManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Register DB
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionStringDB");
            Console.WriteLine($"Connection string: {connectionString}");
            builder.Services.AddDbContext<FUNewsManagementContext>(options =>
                options.UseSqlServer(connectionString));


            //Register repositories
            builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();
            builder.Services.AddScoped<IArticleRepo, ArticleRepo>();
            builder.Services.AddScoped<ITagRepo, TagRepo>();
            builder.Services.AddScoped<ISystemAccountRepo, SystemAccountRepo>();

            //Register services
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IArticleService, ArticleService>();
            builder.Services.AddScoped<ITagService, TagService>();
            builder.Services.AddScoped<ISystemAccountService, SystemAccountService>();

            
            // Add services to the container.
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
