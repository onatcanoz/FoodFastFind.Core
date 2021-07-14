using System;
using System.Collections.Generic;
using System.Text;
using AppCore.DataAccess.Bases.EntityFramework;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.EntityFrameWork.Repositories
{
    public abstract class MaterialRepositoryBase : RepositoryBase<Material>
    {
        protected MaterialRepositoryBase(DbContext db) : base(db)
        {

        }
    }

    public class MaterialRepository : MaterialRepositoryBase
    {
        public MaterialRepository(DbContext db) : base(db)
        {

        }
    }
}
