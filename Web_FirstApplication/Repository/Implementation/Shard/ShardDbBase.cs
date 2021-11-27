using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_FirstApplication.Models.DbModel.Shard;

namespace Web_FirstApplication.Repository.Implementation.Shard
{
    public class ShardDbBase : DbContext
    {
        public ShardDbBase(DbContextOptions<ShardDbBase> options)
            : base(options)
        {
        }
        public DbSet<_Guild> _Guilds { get; set; }
        public DbSet<_SiegeFortress> _SiegeFortress { get; set; }
        public DbSet<_Char> _Chars { get; set; }
        public DbSet<_CharTrijob> _CharTrijobs { get; set; }
        public DbSet<_TrainingCampMember> _TrainingCampMembers { get; set; }
        public DbSet<_GuildMember> _GuildMembers { get; set; }
    }
}
