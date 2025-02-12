using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace MyBlog.CoreLayer.Utilities
{
    public static class PasswordHasherExtensions
    {
        /// <summary>
        /// هش کردن پسورد با استفاده از BCrypt.
        /// </summary>
        public static string HashPassword(this string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        /// <summary>
        /// بررسی تطابق پسورد ورودی با هش ذخیره‌شده.
        /// </summary>
        public static bool VerifyPassword(this string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}

