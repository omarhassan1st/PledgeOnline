using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Web_FirstApplication.Repository.Declaration.Shared
{
    public interface IDbFunctions<TEntity> where TEntity : class
    {
        TEntity Find(Expression<Func<TEntity, bool>> criteria);
        IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> criteria);
        public IEnumerable<TEntity> ToList();
        public void Add(TEntity entity);
        public int Count();
        public IEnumerable<TEntity> Take(int count);
        public IEnumerable<TEntity> Skip(int count);
        public void Update(TEntity entity);
        public void Remove(int count);
        public TEntity FirstOrDeafult();
        public bool Any(Expression<Func<TEntity, bool>> criteria);
    }
}
