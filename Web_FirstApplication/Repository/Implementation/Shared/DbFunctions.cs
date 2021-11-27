using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Web_FirstApplication.Repository.Declaration.IAccount;
using Web_FirstApplication.Repository.Declaration.Shared;
using Web_FirstApplication.Repository.Implementation.Account;
using Web_FirstApplication.Repository.Implementation.Shard;
using Web_FirstApplication.Repository.Implementation.WebSite;

namespace Web_FirstApplication.Repository.Implementation.Shared
{
    public class DbFunctions<TEntity> : IDbFunctions<TEntity> where TEntity : class
    {
        protected WebSiteDbBase _webSiteContext;
        protected AccountDbBase _accountContext;
        protected ShardDbBase _shardContext;
        private DbSet<TEntity> Table;
        public DbFunctions(WebSiteDbBase webSiteContext)
        {
            _webSiteContext = webSiteContext;
            Table = _webSiteContext.Set<TEntity>();

        }
        public DbFunctions(AccountDbBase accountContext)
        {
            _accountContext = accountContext;
            Table = _accountContext.Set<TEntity>();
        }
        public DbFunctions(ShardDbBase shardContext)
        {
            _shardContext = shardContext;
            Table = _shardContext.Set<TEntity>();
        }
        public void Add(TEntity entity)
        {
            Table.Add(entity);
        }

        public int Count()
        {
            return Table.Count();
        }

        public TEntity Find(Expression<Func<TEntity, bool>> criteria)
        {
            return Table.SingleOrDefault(criteria);
        }
        public IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> criteria)
        {
            return Table.Where(criteria);
        }

        public IEnumerable<TEntity> Skip(int count)
        {
             return Table.Skip(count);
        }

        public IEnumerable<TEntity> Take(int count)
        {
            return Table.Take(count);
        }

        public IEnumerable<TEntity> ToList()
        {
           return Table.ToList();
        }

        public void Update(TEntity entity)
        {
            Table.Update(entity);
        }

        public void Remove(int count)
        {
            Table.Remove(Table.Find(count));
        }

        public TEntity FirstOrDeafult()
        {
            return Table.FirstOrDefault();
        }
        public bool Any(Expression<Func<TEntity, bool>> criteria)
        {
            return Table.Any(criteria);
        }
    }
}
