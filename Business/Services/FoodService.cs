using AppCore.Business.Models.Results;
using AppCore.Business.Services.Bases;
using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.EntityFrameWork.Repositories;
using Entities.Entities;

namespace Business.Services
{
    public interface IFoodService : IService<FoodModel>
    {

    }

    public class FoodService : IFoodService
    {
        private readonly FoodRepositoryBase _foodRepository;
        private readonly FoodMaterialRepositoryBase _foodmaterialRepository;

        public FoodService(FoodRepositoryBase foodRepository, FoodMaterialRepositoryBase foodmaterialRepository)
        {
            _foodRepository = foodRepository;
            _foodmaterialRepository = foodmaterialRepository;
        }

        public Result Add(FoodModel model)
        {

            try
            {
                if (_foodRepository.EntityQuery().Any(f => f.Name.ToUpper() == model.Name.ToUpper().Trim()))
                    return new ErrorResult("Food with the same name exist!");   

                var entity = new Food()
                {
                    CategoryId = model.CategoryId,
                    Detail = model.Detail.Trim(),
                    RecipesMaterials = model.RecipesMaterials.Trim(),
                    CookTime = model.CookTime,
                    Name = model.Name.Trim(),
                    PersonNumber = model.PersonNumber,
                    PhotoURL = model.PhotoURL?.Trim(),
                    VideoURL = model.VideoURL?.Trim(),
                    FoodMaterials = model.MaterialsIds.Select(mIs => new FoodMaterial()
                    {
                        MaterialId = mIs
                    }).ToList()
                };
                _foodRepository.Add(entity);
                return new SuccessResult();
            }
            catch (Exception exc)
            {

                return new ExceptionResult(exc);
            }

        }

        public Result Delete(int id)
        {
            try
            {
                _foodRepository.DeleteEntity(id);
                return new SuccessResult();
            }
            catch (Exception exc)
            {

                return new ExceptionResult(exc);
            }
        }

        public void Dispose()
        {
            _foodRepository?.Dispose();
        }

        public IQueryable<FoodModel> Query()
        {
           
                var query = _foodRepository.EntityQuery("Category").Select(f => new FoodModel()
                {
                    Id = f.Id,
                    Name = f.Name,
                    Detail = f.Detail,
                    Guid = f.Guid,
                    RecipesMaterials = f.RecipesMaterials,
                    PersonNumber = f.PersonNumber,
                    CookTime = f.CookTime,
                    VideoURL = f.VideoURL.Trim(),
                    CategoryId = f.CategoryId,
                    Category = new CategoryModel()
                    {
                        Id = f.Category.Id,
                        Name = f.Category.Name,
                        Guid = f.Category.Guid
                    },
                    Materials = f.FoodMaterials.Select(fm => new MaterialModel()
                    {
                        Id = fm.Material.Id,
                        Name = fm.Material.Name
                    }).ToList(),

                    MaterialsIds = f.FoodMaterials.Select(fm => fm.MaterialId).ToList(),
                    PhotoURL = f.PhotoURL,
                });
                return query;
        }

        public Result Update(FoodModel model)
        {
            try
            {
                if (_foodRepository.Query().Any(f => f.Name.ToUpper() == model.Name.ToUpper().Trim() && f.Id != model.Id))
                    return new ErrorResult("Food with the same name exist!");

                var entity = _foodRepository.EntityQuery(f => f.Id == model.Id, "FoodMaterials").SingleOrDefault();
                if (entity == null)
                {
                    return new ErrorResult("Food Not Found!");
                }

                if (entity.FoodMaterials != null && entity.FoodMaterials.Count > 0)
                {

                    foreach (var material in entity.FoodMaterials)
                    {
                        _foodmaterialRepository.Delete(material, false);
                    }

                    _foodmaterialRepository.Save();
                }
               

                entity.CategoryId = model.CategoryId;
                entity.Detail = model.Detail.Trim();
                entity.RecipesMaterials = model.RecipesMaterials.Trim();
                entity.CookTime = model.CookTime;
                entity.Name = model.Name.Trim();
                entity.PersonNumber = model.PersonNumber;
                entity.PhotoURL = model.PhotoURL?.Trim();
                entity.VideoURL = model.VideoURL?.Trim();
                entity.FoodMaterials = (model.MaterialsIds ?? new List<int>()).Select(fId => new FoodMaterial()
                {
                    MaterialId = fId
                }).ToList();
                    _foodRepository.Update(entity);
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }
    }
}
