using System;
using System.Collections.Generic;
using System.Text;
using AppCore.DataAccess.Bases.EntityFramework;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.EntityFrameWork.Repositories
{
    public abstract class CategoryRepositoryBase : RepositoryBase<Category>
    {
        protected CategoryRepositoryBase(DbContext db) : base(db)
        {

        }
    }

    public class CategoryRepository : CategoryRepositoryBase
    {
        public CategoryRepository(DbContext db) : base(db)
        {

        }
    }

}
