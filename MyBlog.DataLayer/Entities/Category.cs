using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBlog.DataLayer.Entities
{
    public class Category : BaseEntity
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Slug { get; set; }

        public string MetaTag { get; set; }
        public string MetaDescription { get; set; }
        public int? ParentId { get; set; }

        // ویژگی والد (خودارجاعی)
        [ForeignKey("ParentId")]
        public Category ParentCategory { get; set; }

        // ویژگی فرزندان
        public ICollection<Category> ChildCategories { get; set; }

        [InverseProperty("Category")]
        public ICollection<Post> Posts { get; set; }

        [InverseProperty("SubCategory")]
        public ICollection<Post> SubPosts { get; set; }
    }
}
