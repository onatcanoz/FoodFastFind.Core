using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppCore.Business.Models.Results;
using AppCore.Business.Services.Bases;
using Business.Models;
using DataAccess.EntityFrameWork.Repositories;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public interface IMaterialService : IService<MaterialModel>
    {
        public IQueryable<int> GetFoodsWithMaterialId(List<int> materialIdList);
    }

    public class MaterialService : IMaterialService
    {
        private readonly MaterialRepositoryBase _materialRepository;
        private readonly FoodRepositoryBase _foodRepository;
        private readonly FoodMaterialRepositoryBase _foodMaterialRepository;

        public MaterialService(MaterialRepositoryBase materialRepository, FoodMaterialRepositoryBase foodMaterialRepository, FoodRepositoryBase foodRepository)
        {
            _materialRepository = materialRepository;
            _foodMaterialRepository = foodMaterialRepository;
            _foodRepository = foodRepository;
        }

        public Result Add(MaterialModel model)
        {
            if (_materialRepository.Query().Any(m => m.Name.ToUpper() == model.Name.ToUpper().Trim()))
                return new ErrorResult("Material with the same name exist!");

            var entity = new Material()
            {
                Name = model.Name,
            };
            _materialRepository.Add(entity);
            return new SuccessResult();
        }

        public Result Delete(int id)
        {
            try
            {
                _materialRepository.DeleteEntity(id);
                return new SuccessResult();
            }
            catch (Exception exc)
            {

                return new ExceptionResult(exc);
            }
        }

        public void Dispose()
        {
            _materialRepository?.Dispose();
        }

        public IQueryable<int> GetFoodsWithMaterialId(List<int> malzeme)
        {
            var foodIdList = _foodMaterialRepository
                .Query()
                .Include(x => x.Food)
                .Where(x => malzeme.Contains(x.MaterialId))
                .GroupBy(x => x.FoodId)
                .Where(x => x.Count() > (malzeme.Count - 1))
                .Select(x => new FoodMaterial()
                {
                    FoodId = x.Key,
                })
                .Select(x=>x.FoodId)
                .AsQueryable();
                

            return foodIdList;
        }

        public IQueryable<MaterialModel> Query()
        {
            return _materialRepository.Query().OrderBy(m => m.Name).Select(m => new MaterialModel()
            {
                Id = m.Id,
                Name = m.Name,
                Foods = m.FoodMaterials.Select(fm => new FoodModel()
                {
                    Id = fm.Food.Id,
                    Name = fm.Food.Name,
                    PhotoURL = fm.Food.PhotoURL
                }).ToList()
            });
        }

        public Result Update(MaterialModel model)
        {
            try
            {
                if (_materialRepository.Query().Any(m => m.Name.ToUpper() == model.Name.ToUpper().Trim() && m.Id != model.Id))
                    return new ErrorResult("Material with the same name exist!");

                var entity = new Material()
                {
                    Id = model.Id,
                    Name = model.Name,
                };

                _materialRepository.Update(entity);
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }
    }
}
