using System.Linq;
using MyBlog.CoreLayer.DTOs.Posts;
using MyBlog.CoreLayer.Mappers;
using MyBlog.CoreLayer.Services.FileManager;
using MyBlog.CoreLayer.Utilities;
using MyBlog.DataLayer.Context;
using Microsoft.EntityFrameworkCore;

namespace MyBlog.CoreLayer.Services.Posts
{
    public class PostService : IPostService
    {
        private readonly BlogContext _context;
        private readonly IFileManager _fileManager;

        public PostService(BlogContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        public OperationResult CreatePost(CreatePostDto command)
        {
            if (command == null || command.ImageFile == null)
                return OperationResult.Error("اطلاعات پست یا تصویر ارسال نشده است");

            var post = PostMapper.MapCreateDtoToPost(command);

            if (IsSlugExist(post.Slug))
                return OperationResult.Error("Slug تکراری است");

            // ذخیره فایل تصویر
            post.ImageName = _fileManager.SaveFileAndReturnName(command.ImageFile, Directories.PostImage);

            _context.Posts.Add(post);
            _context.SaveChanges();

            return OperationResult.Success();
        }

        public OperationResult EditPost(EditPostDto command)
        {
            if (command == null)
                return OperationResult.Error("اطلاعات ارسال شده نامعتبر است");

            var post = _context.Posts.FirstOrDefault(c => c.Id == command.PostId);
            if (post == null)
                return OperationResult.NotFound("پست مورد نظر یافت نشد");

            var oldImage = post.ImageName;
            var newSlug = command.Slug.ToSlug();

            if (post.Slug != newSlug && IsSlugExist(newSlug))
                return OperationResult.Error("Slug تکراری است");

            // به‌روزرسانی اطلاعات پست
            PostMapper.EditPost(command, post);

            // در صورت آپلود تصویر جدید، آن را ذخیره و تصویر قدیمی را حذف می‌کنیم
            if (command.ImageFile != null)
            {
                post.ImageName = _fileManager.SaveFileAndReturnName(command.ImageFile, Directories.PostImage);
            }

            _context.SaveChanges();

            // حذف تصویر قدیمی در صورت آپلود تصویر جدید
            if (command.ImageFile != null)
            {
                _fileManager.DeleteFile(oldImage, Directories.PostImage);
            }

            return OperationResult.Success();
        }

        public PostDto GetPostById(int postId)
        {
            var post = _context.Posts
                .Include(c => c.Category)
                .Include(c => c.SubCategory)
                .FirstOrDefault(c => c.Id == postId);
            return PostMapper.MapToDto(post);
        }

        public PostFilterDto GetPostsByFilter(PostFilterParams filterParams)
        {
            var result = _context.Posts
                .Include(d => d.Category)
                .Include(d => d.SubCategory)
                .OrderByDescending(d => d.CreationDate)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterParams.CategorySlug))
                result = result.Where(r => r.Category.Slug == filterParams.CategorySlug);

            if (!string.IsNullOrWhiteSpace(filterParams.Title))
                result = result.Where(r => r.Title.Contains(filterParams.Title));

            var skip = (filterParams.PageId - 1) * filterParams.Take;
            var pageCount = (int)System.Math.Ceiling(result.Count() / (double)filterParams.Take);

            return new PostFilterDto()
            {
                Posts = result.Skip(skip).Take(filterParams.Take)
                                     .Select(post => PostMapper.MapToDto(post))
                                     .ToList(),
                FilterParams = filterParams,
                PageCount = pageCount
            };
        }

        public bool IsSlugExist(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
                return false;

            return _context.Posts.Any(p => p.Slug == slug.ToSlug());
        }
    }
}
