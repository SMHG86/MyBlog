using System;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.DataLayer.Entities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        // استفاده از UtcNow برای ثبت زمان به صورت استاندارد
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public bool IsDelete { get; set; }
    }
}
