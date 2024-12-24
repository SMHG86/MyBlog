using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.DataLayer.Entities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public bool IsDelete { get; set; }
    }
}