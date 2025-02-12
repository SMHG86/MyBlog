using System;
using System.ComponentModel.DataAnnotations;
using MyBlog.CoreLayer.DTOs.Categories;
using MyBlog.Web.Areas.Admin.Models;
using MyBlog.Web.Areas.Admin;
namespace MyBlog.web.Areas.Admin.Models
{
    public class DashboardViewModel
    {
        public int CategoryCount { get; set; }
        public int PostCount { get; set; }
        public int UserCount { get; set; }
    }
}
