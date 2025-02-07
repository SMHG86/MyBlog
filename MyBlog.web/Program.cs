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

// ثبت سرویس‌های Razor Pages و MVC
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

// ثبت سرویس‌های سفارشی (Dependency Injection)
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddTransient<IFileManager, FileManager>();

// تنظیم DbContext با SQL Server و تعیین Assembly مربوط به Migrations
builder.Services.AddDbContext<BlogContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Default"),
        b => b.MigrationsAssembly("MyBlog.DataLayer"));
});

// پیکربندی احراز هویت با کوکی
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.LogoutPath = "/Auth/Logout";
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.SlidingExpiration = true;
});

// پیکربندی Rate Limiter به صورت Global
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter("global", _ =>
            new FixedWindowRateLimiterOptions
            {
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 5
            }));
});

// فعال‌سازی فشرده‌سازی پاسخ‌ها
builder.Services.AddResponseCompression();

var app = builder.Build();

// تنظیم صفحه‌های خطا و HSTS در محیط Production
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// استفاده از HTTPS و فشرده‌سازی پاسخ‌ها
app.UseHttpsRedirection();
// توجه: اگر می‌خواهید فایل‌های استاتیک هم فشرده شوند، بهتر است UseResponseCompression را قبل از UseStaticFiles فراخوانی کنید.
app.UseResponseCompression();
app.UseStaticFiles();

app.UseRouting();

// اضافه کردن middleware برای Content-Security-Policy (CSP)
// توجه: در صورت امکان بهتر است از nonce یا hash به جای 'unsafe-inline' استفاده کنید.
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; script-src 'self'; style-src 'self' 'unsafe-inline'; img-src 'self';");
    await next();
});

// استفاده از Rate Limiter به عنوان اولین Middleware بعد از Routing (برای محافظت زودهنگام)
app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
