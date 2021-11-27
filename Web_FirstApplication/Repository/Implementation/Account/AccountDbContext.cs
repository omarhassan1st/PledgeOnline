using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_FirstApplication.Models.DbModel.Account;
using Web_FirstApplication.Repository.Declaration.IAccount;
using Web_FirstApplication.Repository.Declaration.Shared;
using Web_FirstApplication.Repository.Implementation.Shared;

namespace Web_FirstApplication.Repository.Implementation.Account
{
    public class AccountDbContext : IAccountDbContext
    {
        private readonly AccountDbBase _DbContext;
        public IDbFunctions<Guild> Guilds { get; set; }
        public IDbFunctions<TB_User> TB_Users { get; set; }
        public IDbFunctions<_ShardCurrentUser> ShardCurrentUsers { get; set; }
        public IDbFunctions<SK_Silk> SK_Silks { get; set; }

        public AccountDbContext(AccountDbBase DbContext)
        {
            _DbContext = DbContext;
            Guilds = new DbFunctions<Guild>(_DbContext);
            TB_Users = new DbFunctions<TB_User>(_DbContext);
            ShardCurrentUsers = new DbFunctions<_ShardCurrentUser>(_DbContext);
            SK_Silks = new DbFunctions<SK_Silk>(_DbContext);
        }

        public int Complete()
        {
            return _DbContext.SaveChanges();
        }

        public void Dispose()
        {
            _DbContext.Dispose();
        }
    }
}
