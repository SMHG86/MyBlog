using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.DataLayer.Entities
{
    public class User : BaseEntity
    {
        [Required]
        public string UserName { get; set; }

        public string FullName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }

        public UserRole Role { get; set; }

        #region Relations

        public ICollection<Post> Posts { get; set; }
        public ICollection<PostComment> PostComments { get; set; }

        #endregion
    }

    public enum UserRole
    {
        Admin,
        User,
        Writer
    }
}
