using System;
using System.Linq;
using MyBlog.CoreLayer.DTOs.Users;
using MyBlog.CoreLayer.Utilities;
using MyBlog.DataLayer.Context;
using MyBlog.DataLayer.Entities;

namespace MyBlog.CoreLayer.Services.Users
{
    public class UserService : IUserService
    {
        private readonly BlogContext _context;

        public UserService(BlogContext context)
        {
            _context = context;
        }

        public OperationResult RegisterUser(UserRegisterDto registerDto)
        {
            if (registerDto == null)
                return OperationResult.Error("اطلاعات ثبت نام نامعتبر است");

            bool isUserNameExist = _context.Users.Any(u => u.UserName == registerDto.UserName);
            if (isUserNameExist)
                return OperationResult.Error("نام کاربری تکراری است");

            // هشینگ امن پسورد با استفاده از BCrypt
            string passwordHash = registerDto.Password.HashPassword();
            _context.Users.Add(new User()
            {
                FullName = registerDto.Fullname,
                UserName = registerDto.UserName,
                Password = passwordHash,
                Role = UserRole.User,
                IsDelete = false,
                CreationDate = DateTime.UtcNow
            });
            _context.SaveChanges();
            return OperationResult.Success();
        }

        public UserDto LoginUser(LoginUserDto loginDto)
        {
            if (loginDto == null)
                return null;

            // جستجو بر اساس UserName
            User user = _context.Users.FirstOrDefault(u => u.UserName == loginDto.UserName);
            if (user == null)
                return null;

            // اعتبارسنجی پسورد با استفاده از BCrypt
            if (!loginDto.Password.VerifyPassword(user.Password))
                return null;

            return new UserDto()
            {
                UserId = user.Id,
                FullName = user.FullName,
                UserName = user.UserName,
                Password = user.Password, 
                Role = user.Role,
                RegisterDate = user.CreationDate
            };
        }
    }
}
