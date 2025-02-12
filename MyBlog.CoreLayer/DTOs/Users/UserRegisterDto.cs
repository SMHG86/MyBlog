using System.ComponentModel.DataAnnotations;

namespace MyBlog.CoreLayer.DTOs.Users
{
    public class UserRegisterDto
    {
        [Required(ErrorMessage = "وارد کردن {0} الزامی است")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "وارد کردن {0} الزامی است")]
        public string Fullname { get; set; }

        [Required(ErrorMessage = "وارد کردن {0} الزامی است")]
        public string Password { get; set; }

        [Required(ErrorMessage = "وارد کردن {0} الزامی است")]
        [EmailAddress(ErrorMessage = "ایمیل معتبر وارد کنید")]
        public string Email { get; set; }
    }
}
