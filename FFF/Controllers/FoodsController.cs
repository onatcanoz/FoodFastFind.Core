using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.AspNetCore.Http;
using MvcWebUI.Settings;

namespace FFF.Controllers
{
    //[Authorize]
    public class FoodsController : Controller
    {
        private readonly IFoodService _foodService;
        private readonly ICategoryService _categoryService;
        private readonly IMaterialService _materialService;

        public FoodsController(IFoodService foodService, ICategoryService categoryService, IMaterialService materialService)
        {
            _foodService = foodService;
            _categoryService = categoryService;
            _materialService = materialService;
        }

        // GET: Foods
        public IActionResult Index()
        {
            var model = _foodService.Query().ToList();
            return View(model);
        }

        // GET: Foods/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var food = _foodService.Query().SingleOrDefault(f => f.Id == id.Value);
            if (food == null)
            {
                return NotFound();
            }

            return View(food);
        }

        // GET: Foods/Create
        //[Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_categoryService.Query().ToList(), "Id", "Name");
            ViewBag.Materials = new MultiSelectList(_materialService.Query().ToList(), "Id", "Name");
            FoodModel model = new FoodModel();
            return View(model);
        }

        // POST: Foods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "admin")]
        public IActionResult Create(FoodModel food, IFormFile image)
        {
            Result foodResult;
            IQueryable<CategoryModel> categoryQuery;
            if (ModelState.IsValid)
            {

                string fileName = null;
                string fileExtension = null;
                string filePath = null; // sunucuda dosyayı kaydedeceğim yol
                bool saveFile = false; // flag
                if (image != null && image.Length > 0)
                {
                    fileName = image.FileName; // asusrog.jpg
                    fileExtension = Path.GetExtension(fileName); // .jpg
                    string[] appSettingsAcceptedImageExtensions = AppSettings.AcceptedImageExtensions.Split(',');
                    bool acceptedImageExtension = false; // flag
                    foreach (string appSettingsAcceptedImageExtension in appSettingsAcceptedImageExtensions)
                    {
                        if (fileExtension.ToLower() == appSettingsAcceptedImageExtension.ToLower().Trim())
                        {
                            acceptedImageExtension = true;
                            break;
                        }
                    }
                    if (!acceptedImageExtension)
                    {
                        ModelState.AddModelError("", "The image extension is not allowed, the accepted image extensions are " + AppSettings.AcceptedImageExtensions);
                        categoryQuery = _categoryService.Query();
                        ViewBag.Categories = new SelectList(categoryQuery.ToList(), "Id", "Name", food.CategoryId);
                        return View(food);
                    }

                    // 1 byte = 8 bits
                    // 1 kilobyte = 1024 bytes
                    // 1 megabyte = 1024 kilobytes = 1024 * 1024 bytes
                    double acceptedFileLength = AppSettings.AcceptedImageMaximumLength * Math.Pow(1024, 2); // bytes
                    if (image.Length > acceptedFileLength)
                    {
                        ModelState.AddModelError("", "The image size is not allowed, the accepted image size must be maximum " + AppSettings.AcceptedImageMaximumLength + " MB");
                        categoryQuery = _categoryService.Query();
                        ViewBag.Categories = new SelectList(categoryQuery.ToList(), "Id", "Name", food.CategoryId);
                        return View(food);
                    }

                    saveFile = true;
                }

                if (saveFile)
                {
                    fileName = Guid.NewGuid() + fileExtension; // x345f-dert5-gfds2-6hjkl.jpg

                    filePath = Path.Combine("wwwroot", "files", "foods", fileName); // .NET Core
                    // .NET Framework: Server.MapPath("~/wwwroot/files/products/x345f-dert5-gfds2-6hjkl.jpg")

                    // wwwroot/files/products/x345f-dert5-gfds2-6hjkl.jpg (sanal yol: virtual path)
                    // D:\BilgeAdam\ETradeCoreBilgeAdam\MvcWebUI\wwwroot\files\products\x345f-dert5-gfds2-6hjkl.jpg (fiziksel yol: physical, absolute path)
                }
                food.PhotoURL = fileName;

                foodResult = _foodService.Add(food);
                if (foodResult.Status == ResultStatus.Exception) // exception
                {
                    throw new Exception(foodResult.Message);
                }
                if (foodResult.Status == ResultStatus.Success) // success
                {
                    if (saveFile)
                    {
                        // .NET Core:
                        using (FileStream fileStream = new FileStream(filePath, FileMode.CreateNew))
                        {
                            image.CopyTo(fileStream);
                        }

                        // .NET Framework:
                        // image.SaveAs(filePath); // image: HttpPostedFileBase
                    }

                    //return RedirectToAction("Index");
                    return RedirectToAction(nameof(Index)); // nameof(Index) = "Index"
                }

                // error
                //ViewBag.Message = productResult.Message;
                ModelState.AddModelError("", foodResult.Message);

                categoryQuery = _categoryService.Query();
                ViewBag.Categories = new SelectList(categoryQuery.ToList(), "Id", "Name", food.CategoryId);
                return View(food);
            }

            // validation error
            categoryQuery = _categoryService.Query();
            ViewBag.Categories = new SelectList(categoryQuery.ToList(), "Id", "Name", food.CategoryId);
            return View(food);
        }


        // GET: Foods/Edit/5
        //[Authorize(Roles = "admin")]
        public IActionResult Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            //var food = _foodService.Query().SingleOrDefault(f => f.Id == id);
            var foodQuery = _foodService.Query();
            var food = foodQuery.SingleOrDefault(p => p.Id == id.Value);
            if (food == null)
            {
                return NotFound();
            }
            ViewBag.Categories = new SelectList(_categoryService.Query().ToList(), "Id", "Name", food.CategoryId);
            ViewBag.Materials = new MultiSelectList(_materialService.Query().ToList(), "Id", "Name", food.MaterialsIds);

            return View(food);
        }

        // POST: Foods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "admin")]
        public IActionResult Edit(FoodModel food, IFormFile image)
        {

            //if (ModelState.IsValid)
            //{
            //    var foodResult = _foodService.Update(food);
            //    if (foodResult.Status == ResultStatus.Exception)
            //        throw new Exception(foodResult.Message);
            //    if (foodResult.Status == ResultStatus.Success)
            //         return RedirectToAction(nameof(Index));
            //    ModelState.AddModelError("", foodResult.Message);
            //}
            //ViewBag.Categories = new SelectList(_categoryService.Query().ToList(), "Id", "Name", food.CategoryId);

            //ViewBag.Materials = new MultiSelectList(_materialService.Query().ToList(), "Id", "Name", food.MaterialsIds);
            //return View(food);
            {
                Result foodResult;
                IQueryable<CategoryModel> categoryQuery;
                if (ModelState.IsValid)
                {

                    string fileName = null;
                    string fileExtension = null;
                    string filePath = null;
                    bool saveFile = false;
                    if (image != null && image.Length > 0)
                    {
                        fileName = image.FileName;
                        fileExtension = Path.GetExtension(fileName);
                        string[] appSettingsAcceptedImageExtensions = AppSettings.AcceptedImageExtensions.Split(',');
                        bool acceptedImageExtension = false;
                        foreach (string appSettingsAcceptedImageExtension in appSettingsAcceptedImageExtensions)
                        {
                            if (fileExtension.ToLower() == appSettingsAcceptedImageExtension.ToLower().Trim())
                            {
                                acceptedImageExtension = true;
                                break;
                            }
                        }
                        if (!acceptedImageExtension)
                        {
                            ModelState.AddModelError("", "The image extension is not allowed, the accepted image extensions are " + AppSettings.AcceptedImageExtensions);
                            categoryQuery = _categoryService.Query();
                            ViewBag.Categories = new SelectList(categoryQuery.ToList(), "Id", "Name", food.CategoryId);
                            return View(food);
                        }

                        double acceptedFileLength = AppSettings.AcceptedImageMaximumLength * Math.Pow(1024, 2); // bytes
                        if (image.Length > acceptedFileLength)
                        {
                            ModelState.AddModelError("", "The image size is not allowed, the accepted image size must be maximum " + AppSettings.AcceptedImageMaximumLength + " MB");
                            categoryQuery = _categoryService.Query();
                            ViewBag.Categories = new SelectList(categoryQuery.ToList(), "Id", "Name", food.CategoryId);
                            return View(food);
                        }

                        saveFile = true;
                    }

                    var existingFood = _foodService.Query().SingleOrDefault(p => p.Id == food.Id);
                    if (string.IsNullOrWhiteSpace(existingFood.PhotoURL) && saveFile)
                    {
                        fileName = Guid.NewGuid() + fileExtension;
                    }
                    else // existingProduct.ImageFileName = x345f-dert5-gfds2-6hjkl.jpg, fileExtension = png
                    {
                        int periodIndex = existingFood.PhotoURL.IndexOf("."); // 23
                        fileName = existingFood.PhotoURL.Substring(0, periodIndex + 1); // x345f-dert5-gfds2-6hjkl.
                        fileName = fileName + fileExtension; // x345f-dert5-gfds2-6hjkl.png
                    }

                    food.PhotoURL = fileName;

                    foodResult = _foodService.Update(food);
                    if (foodResult.Status == ResultStatus.Exception) // exception
                    {
                        throw new Exception(foodResult.Message);
                    }
                    if (foodResult.Status == ResultStatus.Success) // success
                    {
                        if (saveFile)
                        {
                            filePath = Path.Combine("wwwroot", "files", "foods", fileName);
                            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                image.CopyTo(fileStream);
                            }
                        }

                        //return RedirectToAction("Index");
                        return RedirectToAction(nameof(Index)); // nameof(Index) = "Index"
                    }

                    // error
                    //ViewBag.Message = productResult.Message;
                    ModelState.AddModelError("", foodResult.Message);

                    categoryQuery = _categoryService.Query();
                    ViewBag.Categories = new SelectList(categoryQuery.ToList(), "Id", "Name", food.CategoryId);
                    return View(food);
                }

                // validation error
                categoryQuery = _categoryService.Query();
                ViewBag.Categories = new SelectList(categoryQuery.ToList(), "Id", "Name", food.CategoryId);
                return View(food);
            }
        }

        // GET: Foods/Delete/5
        [Authorize(Roles = "admin")]
        public IActionResult Delete(int? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var foodResult = _foodService.Delete(id.Value);
            //if (foodResult.Status == ResultStatus.Exception)
            //    throw new Exception(foodResult.Message);


            //return RedirectToAction(nameof(Index));

            if (!id.HasValue)
                return NotFound();

            var existingFood = _foodService.Query().SingleOrDefault(p => p.Id == id.Value);

            var result = _foodService.Delete(id.Value);
            if (result.Status == ResultStatus.Success)
            {
                if (!string.IsNullOrWhiteSpace(existingFood.PhotoURL))
                {
                    string filePath = Path.Combine("wwwroot", "files", "products", existingFood.PhotoURL);
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                }

                return RedirectToAction(nameof(Index));
            }
            throw new Exception(result.Message);
        }

        public IActionResult DeleteFoodImage(int? id)
        {
            if (id == null)
                return NotFound();

            var existingFood = _foodService.Query().SingleOrDefault(f => f.Id == id.Value);
            if (!string.IsNullOrWhiteSpace(existingFood.PhotoURL))
            {
                string filePath = Path.Combine("wwwroot", "files", "foods", existingFood.PhotoURL);
                existingFood.PhotoURL = null;
                var result = _foodService.Update(existingFood);
                if (result.Status == ResultStatus.Exception)
                    throw new Exception(result.Message);
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }

            return View(nameof(Details), existingFood);
        }

        //// POST: Foods/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var food = await _context.Foods.FindAsync(id);
        //    _context.Foods.Remove(food);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}
    }
}
