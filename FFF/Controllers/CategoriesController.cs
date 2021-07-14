using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppCore.Business.Models.Results;
using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess.EntityFrameWork.Context;
using Entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

namespace FFF.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        // GET: Categories
        public IActionResult Index()
        {
            var model = _categoryService.Query().ToList();
            return View(model);
        }

        // GET: Categories/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _categoryService.Query().SingleOrDefault(c => c.Id == id.Value);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        [Authorize(Roles = "admin")]

        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_categoryService.Query().ToList(), "Id", "Name");
            CategoryModel model = new CategoryModel();
            return View(model);
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]

        public IActionResult Create(CategoryModel category , List<int> Foods)
        {
            if (ModelState.IsValid)
            {
                var result = _categoryService.Add(category);
                if (result.Status == ResultStatus.Exception)
                    throw new Exception(result.Message);
                if (result.Status == ResultStatus.Success)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", result.Message);
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        [Authorize(Roles = "admin")]
        public IActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }
                var category = _categoryService.Query().SingleOrDefault(c => c.Id == id);
                if (category == null)
                {
                    return NotFound();
                }
                return View(category);
            }
            catch (Exception exc)
            {

                throw new Exception("Error occurred while editing");
            }


        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public IActionResult Edit(CategoryModel category)
        {
            if (ModelState.IsValid)
            {
                var categoryResult = _categoryService.Update(category);
                if (categoryResult.Status == ResultStatus.Exception)
                    throw new Exception(categoryResult.Message);
                if (categoryResult.Status == ResultStatus.Success)
                    return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", categoryResult.Message);
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        [Authorize(Roles = "admin")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryResult = _categoryService.Delete(id.Value);
            if (categoryResult.Status == ResultStatus.Exception)
                return View("NopeDelete");


            return RedirectToAction(nameof(Index));
        }

        // POST: Categories/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var category = await _context.Categories.FindAsync(id);
        //    _context.Categories.Remove(category);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool CategoryExists(int id)
        //{
        //    return _context.Categories.Any(e => e.Id == id);
        //}
    }
}
