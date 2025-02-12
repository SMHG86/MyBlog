using Microsoft.AspNetCore.Mvc;
using MyBlog.CoreLayer.Services.Categories;
using MyBlog.CoreLayer.Services.Posts;
using MyBlog.CoreLayer.Services.Users;
using MyBlog.web.Areas.Admin.Models;
using MyBlog.Web.Areas.Admin.Models;

namespace MyBlog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IPostService _postService;
        private readonly IUserService _userService;

        public DashboardController(ICategoryService categoryService,
                                   IPostService postService,
                                   IUserService userService)
        {
            _categoryService = categoryService;
            _postService = postService;
            _userService = userService;
        }

        public IActionResult Index()
        {
            // جمع‌آوری آمار از سرویس‌ها (در اینجا برای مقالات، دسته‌بندی‌ها و کاربران)
            var model = new DashboardViewModel
            {
                CategoryCount = _categoryService.GetAllCategory().Count,
                PostCount = _postService.GetPostsByFilter(new CoreLayer.DTOs.Posts.PostFilterParams { PageId = 1, Take = int.MaxValue }).Posts.Count,
               // UserCount = _userService.GetAllUsers().Count 
            };

            return View(model);
        }
    }
}
