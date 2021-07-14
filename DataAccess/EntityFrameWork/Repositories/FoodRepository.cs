using AppCore.DataAccess.Bases;
using AppCore.DataAccess.Bases.EntityFramework;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.EntityFrameWork.Repositories
{
    public abstract class FoodRepositoryBase : RepositoryBase<Food>
    {
        protected FoodRepositoryBase(DbContext db) : base(db)
        {

        }
    }

    public class FoodRepository : FoodRepositoryBase
    {
        public FoodRepository(DbContext db) : base(db)
        {

        }
    }
}
