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
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace FFF.Controllers
{
    public class MaterialsController : Controller
    {
        private readonly IMaterialService _materialService;
        private readonly IFoodService _foodService;

        public MaterialsController(IMaterialService materialService, IFoodService foodService)
        {
            _materialService = materialService;
            _foodService = foodService;
        }


        // GET: Materials
        public IActionResult Index()
        {
            var materialList = _materialService.Query().ToList();
            return View(materialList);
        }

        // GET: Materials/Details/5
        //[Authorize(Roles = "User")]
        [Authorize]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var material = _materialService.Query().SingleOrDefault(m => m.Id == id.Value);
            if (material == null)
            {
                return NotFound();
            }

            return View(material);
        }

        // GET: Materials/Create
        //[Authorize(Users = "onat@gmail.com,onatcanoz@hotmail.com")]
        //[Authorize(Roles = "User")]
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            ViewBag.Materials = new SelectList(_materialService.Query().ToList(), "Id", "Name");
            MaterialModel model = new MaterialModel();

            return View(model);
        }


      
        public IActionResult GetFoodList(List<int> malzeme)
        {
            var foodIdList = _materialService.GetFoodsWithMaterialId(malzeme).ToList();

            var foodList = _foodService.Query().Where(x => foodIdList.Contains(x.Id)).ToList();

            return Json(foodList);
        }

        //public string GetFoodList(string malzeme)
        //{
        //    var foodList = _foodService.Query().Include(i => i.Materials).Where(i => i.Materials.Any(i => i.Name == malzeme)).ToList();
        //    string res = JsonConvert.SerializeObject(foodList);
        //    return res;
        //}


        // POST: Materials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Users = "onat@gmail.com,onatcanoz@hotmail.com")]
        //[Authorize(Roles = "Admin")]
        [Authorize(Roles = "admin")]
        public IActionResult Create(MaterialModel material)
        {
            if (ModelState.IsValid)
            {
                var result = _materialService.Add(material);
                if (result.Status == ResultStatus.Exception)
                    throw new Exception(result.Message);
                if (result.Status == ResultStatus.Success)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", result.Message);
            }
            return View(material);
        }


        // GET: Materials/Edit/5
        //[Authorize(Roles = "User")]
        [Authorize(Roles = "admin")]
        public IActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }
                var material = _materialService.Query().SingleOrDefault(m => m.Id == id);
                if (material == null)
                {
                    return NotFound();
                }
                return View(material);
            }
            catch (Exception exc)
            {

                throw new Exception("Error occurred while editing");
            }
        }

        // POST: Materials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        [Authorize(Roles = "admin")]
        public IActionResult Edit(MaterialModel material)
        {
            if (ModelState.IsValid)
            {
                var materialResult = _materialService.Update(material);
                if (materialResult.Status == ResultStatus.Exception)
                    throw new Exception(materialResult.Message);
                if (materialResult.Status == ResultStatus.Success)
                    return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", materialResult.Message);
            }
            return View(material);
        }

        // GET: Materials/Delete/5
        //[Authorize(Roles = "Admin")]
        [Authorize(Roles = "admin")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materialResult = _materialService.Delete(id.Value);
            if (materialResult.Status == ResultStatus.Exception)
                return View("NopeDelete");


            return RedirectToAction(nameof(Index));
        }

        //// POST: Materials/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var material = await _context.Materials.FindAsync(id);
        //    _context.Materials.Remove(material);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool MaterialExists(int id)
        //{
        //    return _context.Materials.Any(e => e.Id == id);
        //}
    }
}
