using System;
using System.Security.Claims;

namespace MyBlog.CoreLayer.Utilities
{
    public static class UserUtil
    {
        public static int GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            var claimValue = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(claimValue, out int userId))
                return userId;

            throw new Exception("شناسه کاربر در توکن موجود نیست");
        }
    }
}
