using MyBlog.CoreLayer.DTOs.Users;
using MyBlog.CoreLayer.Utilities;
using MyBlog.DataLayer.Entities;

namespace MyBlog.CoreLayer.Services.Users
{
    public interface IUserService
    {
        OperationResult RegisterUser(UserRegisterDto registerDto);
        UserDto LoginUser(LoginUserDto loginDto);
    }
}