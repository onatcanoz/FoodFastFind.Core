using System;
using System.Collections.Generic;
using System.Text;
using AppCore.DataAccess.Bases.EntityFramework;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.EntityFrameWork.Repositories
{
    public abstract class FoodMaterialRepositoryBase : RepositoryBase<FoodMaterial>
    {
        protected FoodMaterialRepositoryBase(DbContext db) : base(db)
        {

        }
    }

    public class FoodMaterialRepository : FoodMaterialRepositoryBase
    {
        public FoodMaterialRepository(DbContext db) : base(db)
        {

        }
    }
}
