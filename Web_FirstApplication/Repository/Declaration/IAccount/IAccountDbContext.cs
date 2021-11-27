using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_FirstApplication.Models.DbModel.Account;
using Web_FirstApplication.Repository.Declaration.Shared;

namespace Web_FirstApplication.Repository.Declaration.IAccount
{
    public interface IAccountDbContext
    {
        IDbFunctions<Guild> Guilds { get; set; }
        IDbFunctions<TB_User> TB_Users { get; set; }
        IDbFunctions<_ShardCurrentUser> ShardCurrentUsers { get; set; }
        IDbFunctions<SK_Silk> SK_Silks { get; set; }

        public int Complete();
    }
}
