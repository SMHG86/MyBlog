﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBlog.CoreLayer.DTOs.Categories;
using MyBlog.CoreLayer.Services.Categories;
using MyBlog.CoreLayer.Utilities;
using MyBlog.Web.Areas.Admin.Models.Categories;

namespace MyBlog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public IActionResult Index()
        {
            return View(_categoryService.GetAllCategory());
        }

        [Route("/admin/category/add/{parentId?}")]
        public IActionResult Add(int? parentId)
        {
            return View();
        }

        [HttpPost("/admin/category/add/{parentId?}")]
        public IActionResult Add(int? parentId,CreateCategoryViewModel createViewModel)
        {
            createViewModel.ParentId = parentId;
            var result = _categoryService.CreateCategory(createViewModel.MapToDto());
            if (result.Status != OperationResultStatus.Success)
            {
                ModelState.AddModelError(nameof(createViewModel.Slug), result.Message);
                return View();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var category = _categoryService.GetCategoryBy(id);
            if (category == null)
                return RedirectToAction("Index");

            var model = new EditCategoryViewModel()
            {
                Slug = category.Slug,
                MetaTag = category.MetaTag,
                MetaDescription = category.MetaDescription,
                Title = category.Title,
                
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, EditCategoryViewModel editModel)
        {
            var result = _categoryService.EditCategory(new EditCategoryDto()
            {
                Slug = editModel.Slug,
                MetaTag = editModel.MetaTag,
                MetaDescription = editModel.MetaDescription,
                Title = editModel.Title,
                Id = id
            });
            if (result.Status != OperationResultStatus.Success)
            {
                ModelState.AddModelError(nameof(editModel.Slug), result.Message);
                return View();
            }
            return RedirectToAction("Index");
        }


        public IActionResult GetChildCategories(int parentId)
        {
            var categoy = _categoryService.GetChildCategories(parentId);

            return new JsonResult(categoy);
        }
    }
}
