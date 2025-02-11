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

            var isUserNameExist = _context.Users.Any(u => u.UserName == registerDto.UserName);

            if (isUserNameExist)
                return OperationResult.Error("نام کاربری تکراری است");

            // توجه: MD5 الگوریتمی ضعیف است؛ توصیه می‌شود از الگوریتم‌های امن‌تری مانند BCrypt یا ASP.NET Core Identity استفاده کنید.
            var passwordHash = registerDto.Password.EncodeToMd5();
            _context.Users.Add(new User()
            {
                FullName = registerDto.Fullname,
                UserName = registerDto.UserName,
                Password = passwordHash,
                Role = UserRole.User,
                IsDelete = false,
                CreationDate = DateTime.UtcNow // استفاده از UTC جهت جلوگیری از مشکلات زمانی
            });
            _context.SaveChanges();
            return OperationResult.Success();
        }

        public UserDto LoginUser(LoginUserDto loginDto)
        {
            if (loginDto == null)
                return null;

            var passwordHashed = loginDto.Password.EncodeToMd5();
            var user = _context.Users
                .FirstOrDefault(u => u.UserName == loginDto.UserName && u.Password == passwordHashed);

            if (user == null)
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
