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
    public interface ICategoryService : IService<CategoryModel>
    {

    }

    public class CategoryService : ICategoryService
    {
        private readonly CategoryRepositoryBase _categoryRepository;

        public CategoryService(CategoryRepositoryBase categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public Result Add(CategoryModel model)
        {
            if (_categoryRepository.Query().Any(c => c.Name.ToUpper() == model.Name.ToUpper().Trim()))
                return new ErrorResult("Category with the same name exist!");

            var entity = new Category()
            {
                Name = model.Name
            };
            _categoryRepository.Add(entity);
            return new SuccessResult();
        }

        public Result Delete(int id)
        {
            try
            {
                _categoryRepository.DeleteEntity(id);
                return new SuccessResult();
            }
            catch (Exception exc)
            {

                return new ExceptionResult(exc);
            }
        }

        public void Dispose()
        {
            _categoryRepository?.Dispose();
        }

        public IQueryable<CategoryModel> Query()
        {
            return _categoryRepository.Query().OrderBy(c => c.Name).Select(c => new CategoryModel()
            {
                Id = c.Id,
                Guid = c.Guid,
                Name = c.Name,
                Foods = c.Foods.Select(f => new FoodModel()
                {
                    Name = f.Name,
                    Id = f.Id,
                    Detail = f.Detail,
                    CategoryId = f.CategoryId
                }).ToList()
            });
        }

        public Result Update(CategoryModel model)
        {
            try
            {
                if (_categoryRepository.Query().Any(c => c.Name.ToUpper() == model.Name.ToUpper().Trim() && c.Id != model.Id))
                    return new ErrorResult("Category with the same name exist!");

                var entity = new Category()
                {
                    Id = model.Id,
                    Name = model.Name
                };

                _categoryRepository.Update(entity);
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }
    }
}
