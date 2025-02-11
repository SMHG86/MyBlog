using MyBlog.CoreLayer.DTOs.Posts;
using MyBlog.CoreLayer.Utilities;
using MyBlog.DataLayer.Entities;

namespace MyBlog.CoreLayer.Mappers
{
    public class PostMapper
    {
        public static Post MapCreateDtoToPost(CreatePostDto dto)
        {
            if (dto == null)
                return null;

            return new Post()
            {
                Title = dto.Title,
                Description = dto.Description,
                CategoryId = dto.CategoryId,
                SubCategoryId = dto.SubCategoryId,
                Slug = dto.Slug.ToSlug(),
                UserId = dto.UserId,
                Visit = 0,
                IsDelete = false
            };
        }

        public static PostDto MapToDto(Post post)
        {
            if (post == null)
                return null;

            return new PostDto()
            {
                PostId = post.Id,
                Title = post.Title,
                Description = post.Description,
                CategoryId = post.CategoryId,
                SubCategoryId = post.SubCategoryId,
                Slug = post.Slug,
                UserId = post.UserId,
                Visit = post.Visit,
                CreationDate = post.CreationDate,
                ImageName = post.ImageName,
                Category = CategoryMapper.Map(post.Category),
                SubCategory = post.SubCategoryId == null ? null : CategoryMapper.Map(post.SubCategory)
            };
        }

        public static Post EditPost(EditPostDto editDto, Post post)
        {
            if (editDto == null || post == null)
                return post;

            post.Title = editDto.Title;
            post.Description = editDto.Description;
            post.CategoryId = editDto.CategoryId;
            post.SubCategoryId = editDto.SubCategoryId;
            post.Slug = editDto.Slug.ToSlug();

            return post;
        }
    }
}
