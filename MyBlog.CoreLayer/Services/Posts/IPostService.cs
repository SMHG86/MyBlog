using MyBlog.CoreLayer.DTOs.Posts;
using MyBlog.CoreLayer.Utilities;
using MyBlog.DataLayer.Entities;

namespace MyBlog.CoreLayer.Services.Posts
{
    public interface IPostService
    {
        OperationResult CreatePost(CreatePostDto command);
        OperationResult EditPost(EditPostDto command);
        PostDto GetPostById(int postId);
        PostFilterDto GetPostsByFilter(PostFilterParams filterParams);
        bool IsSlugExist(string slug);
    }
}