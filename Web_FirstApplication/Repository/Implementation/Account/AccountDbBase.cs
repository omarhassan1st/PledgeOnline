using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_FirstApplication.Models.DbModel.Account;

namespace Web_FirstApplication.Repository.Implementation.Account
{
    public class AccountDbBase : DbContext
    {
        public AccountDbBase(DbContextOptions<AccountDbBase> options)
            : base(options)
        {
        }
        public DbSet<TB_User> TB_Users { get; set; }
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<_ShardCurrentUser> ShardCurrentUsers { get; set; }
        public DbSet<SK_Silk> SK_Silks { get; set; }
    }

}
