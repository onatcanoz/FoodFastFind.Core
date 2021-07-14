using System;
using System.Linq;
using AppCore.Records.Bases;

namespace AppCore.DataAccess.Bases
{
    public interface IRepositoryBase<TEntity> : IDisposable where TEntity : RecordBase, new()
    {
        IQueryable<TEntity> Query();
        void Add(TEntity entity, bool save = true);
        void Update(TEntity entity, bool save = true);
        void Delete(TEntity entity, bool save = true);
        int Save();
    }
}
