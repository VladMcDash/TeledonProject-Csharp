namespace ProiectMPP.repository;

using System.Collections.Generic;

public interface IRepository<TId, TEntity> {
    void Add(TEntity entity);
    void Delete(TId id);
    void Update(TId id, TEntity entity);
    TEntity FindOne(TId id);
    IEnumerable<TEntity> FindAll();
}