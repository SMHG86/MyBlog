using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MyBlog.CoreLayer.Services;
using MyBlog.DataLayer.Context;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using MyBlog.CoreLayer.Services.Categories;
using MyBlog.CoreLayer.Services.FileManager;
using MyBlog.CoreLayer.Services.Posts;
using MyBlog.CoreLayer.Services.Users;

var builder = WebApplication.CreateBuilder(args);

// اضافه کردن Razor Pages و کنترلرها
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

// تزریق وابستگی‌ها (Dependency Injection)
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddTransient<IFileManager, FileManager>();

// تنظیمات دیتابیس و محل مایگریشن‌ها
builder.Services.AddDbContext<BlogContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Default"),
        b => b.MigrationsAssembly("MyBlog.DataLayer") // تنظیم مسیر مایگریشن
    );
});

// تنظیمات احراز هویت (Authentication)
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.LogoutPath = "/Auth/Logout";
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.SlidingExpiration = true;
});

// تنظیمات Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
    {
        return RateLimitPartition.GetFixedWindowLimiter("global", _ =>
            new FixedWindowRateLimiterOptions
            {
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 5
            });
    });
});

// فعال کردن فشرده‌سازی پاسخ‌ها
builder.Services.AddResponseCompression();

var app = builder.Build();

// فعال کردن خطایاب توسعه یا صفحات خطا
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Middlewareها
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// تنظیمات امنیتی برای هدرها
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; script-src 'self'; style-src 'self' 'unsafe-inline'; img-src 'self';");
    await next();
});

// فعال کردن احراز هویت و Rate Limiting
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();
app.UseResponseCompression();

// Map کردن Razor Pages و کنترلرها
app.MapRazorPages();
app.MapControllers();

app.Run();
