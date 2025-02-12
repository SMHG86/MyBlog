using MyBlog.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MyBlog.DataLayer.Context
{
    public class BlogContext : DbContext
    {
        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<PostComment> PostComments { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // تنظیم DeleteBehavior برای تمامی روابط به صورت Restrict
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(s => s.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            // اعمال Global Query Filter برای پیاده‌سازی soft delete
            modelBuilder.Entity<Category>().HasQueryFilter(c => !c.IsDelete);
            modelBuilder.Entity<Post>().HasQueryFilter(p => !p.IsDelete);
            modelBuilder.Entity<PostComment>().HasQueryFilter(pc => !pc.IsDelete);
            modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDelete);

            // ایجاد ایندکس روی فیلدهای پرتکرار
            modelBuilder.Entity<Category>().HasIndex(c => c.Slug);
            modelBuilder.Entity<Post>().HasIndex(p => p.Slug);
            modelBuilder.Entity<User>().HasIndex(u => u.UserName).IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
