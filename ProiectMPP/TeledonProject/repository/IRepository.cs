using System.Collections.Generic;
using ProiectMPP.TeledonProject.domain;

namespace TeledonProject.repository
{
    public interface IRepository<TId, TEntity> where TEntity : Entity<TId>
    {
        void Add(TEntity entity);
        void Delete(TId id);
        void Update(TId id, TEntity entity);
        TEntity FindOne(TId id);
        IEnumerable<TEntity> FindAll();
    }
}