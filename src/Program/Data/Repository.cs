using System.Collections.Generic;

namespace Program.Data
{
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        List<TEntity> GetAll();
        TEntity Add(TEntity entity);
        List<TEntity> AddRange(List<TEntity> entities);
    }

    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        private List<TEntity> Data;

        public Repository()
        {
            Data = new();
        }

        public List<TEntity> GetAll()
        {
            return Data;
        }

        public TEntity Add(TEntity entity)
        {
            Data.Add(entity);
            return entity;
        }

        public List<TEntity> AddRange(List<TEntity> entities)
        {
            Data.AddRange(entities);
            return entities;
        }
    }
}
