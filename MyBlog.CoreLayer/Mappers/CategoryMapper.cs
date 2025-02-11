using MyBlog.CoreLayer.DTOs.Categories;
using MyBlog.DataLayer.Entities;

namespace MyBlog.CoreLayer.Mappers
{
    public class CategoryMapper
    {
        public static CategoryDto Map(Category category)
        {
            if (category == null)
                return null;

            return new CategoryDto()
            {
                Id = category.Id,
                Title = category.Title,
                ParentId = category.ParentId,
                Slug = category.Slug,
                MetaTag = category.MetaTag,
                MetaDescription = category.MetaDescription
            };
        }
    }
}
