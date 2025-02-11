using System;
using System.Collections.Generic;
using System.Linq;
using MyBlog.CoreLayer.DTOs.Categories;
using MyBlog.CoreLayer.Mappers;
using MyBlog.CoreLayer.Utilities;
using MyBlog.DataLayer.Context;
using MyBlog.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace MyBlog.CoreLayer.Services.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly BlogContext _context;

        public CategoryService(BlogContext context)
        {
            _context = context;
        }

        public OperationResult CreateCategory(CreateCategoryDto command)
        {
            if (command == null)
                return OperationResult.Error("اطلاعات ورودی نامعتبر است");

            if (IsSlugExist(command.Slug))
                return OperationResult.Error("Slug تکراری است");

            var category = new Category()
            {
                Title = command.Title,
                ParentId = command.ParentId,
                Slug = command.Slug.ToSlug(),
                MetaTag = command.MetaTag,
                MetaDescription = command.MetaDescription,
                IsDelete = false
            };

            _context.Categories.Add(category);
            _context.SaveChanges(); // در صورت نیاز به بهبود کارایی، می‌توانید از SaveChangesAsync استفاده کنید

            return OperationResult.Success();
        }

        public OperationResult EditCategory(EditCategoryDto command)
        {
            if (command == null)
                return OperationResult.Error("اطلاعات ورودی نامعتبر است");

            var category = _context.Categories.FirstOrDefault(c => c.Id == command.Id);
            if (category == null)
                return OperationResult.NotFound("دسته‌بندی مورد نظر یافت نشد");

            var newSlug = command.Slug.ToSlug();
            if (newSlug != category.Slug && IsSlugExist(newSlug))
                return OperationResult.Error("Slug تکراری است");

            category.Title = command.Title;
            category.Slug = newSlug;
            category.MetaTag = command.MetaTag;
            category.MetaDescription = command.MetaDescription;

            _context.SaveChanges();
            return OperationResult.Success();
        }

        public List<CategoryDto> GetAllCategory()
        {
            return _context.Categories
                           .Select(category => CategoryMapper.Map(category))
                           .ToList();
        }

        public CategoryDto GetCategoryBy(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            return CategoryMapper.Map(category);
        }

        public CategoryDto GetCategoryBy(string slug)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Slug == slug);
            return CategoryMapper.Map(category);
        }

        public List<CategoryDto> GetChildCategories(int parentId)
        {
            return _context.Categories
                           .Where(c => c.ParentId == parentId)
                           .Select(category => CategoryMapper.Map(category))
                           .ToList();
        }

        public bool IsSlugExist(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
                return false;

            return _context.Categories.Any(c => c.Slug == slug.ToSlug());
        }
    }
}
