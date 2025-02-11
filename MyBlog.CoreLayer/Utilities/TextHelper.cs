using System.Text.RegularExpressions;

namespace MyBlog.CoreLayer.Utilities
{
    public static class TextHelper
    {
        public static string ToSlug(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            value = value.Trim().ToLowerInvariant();
            // حذف کاراکترهای نامعتبر: فقط حروف، اعداد و فاصله مجازند
            value = Regex.Replace(value, @"[^a-z0-9\s-]", "");
            // تبدیل چند فاصله یا خط تیره به یک فاصله
            value = Regex.Replace(value, @"[\s-]+", " ").Trim();
            // جایگزینی فاصله با خط تیره
            return Regex.Replace(value, @"\s", "-");
        }
    }
}
