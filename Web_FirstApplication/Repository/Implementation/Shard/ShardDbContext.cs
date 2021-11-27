using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_FirstApplication.Models.DbModel.Shard;
using Web_FirstApplication.Repository.Declaration.IAccount;
using Web_FirstApplication.Repository.Declaration.IShard;
using Web_FirstApplication.Repository.Declaration.Shared;
using Web_FirstApplication.Repository.Implementation.Shared;

namespace Web_FirstApplication.Repository.Implementation.Shard
{
    public class ShardDbContext : IShardDbContext
    {
        private readonly ShardDbBase _DbContext;
        public IDbFunctions<_Guild> _Guilds { get; set; }
        public IDbFunctions<_SiegeFortress> _SiegeFortress { get; set; }
        public IDbFunctions<_Char> _Chars { get; set; }
        public IDbFunctions<_CharTrijob> _CharTrijobs { get; set; }
        public IDbFunctions<_TrainingCampMember> _TrainingCampMembers { get; set; }
        public IDbFunctions<_GuildMember> _GuildMembers { get; set; }

        public ShardDbContext(ShardDbBase DbContext)
        {
            _DbContext = DbContext;
            _Guilds = new DbFunctions<_Guild>(_DbContext);
            _SiegeFortress = new DbFunctions<_SiegeFortress>(_DbContext);
            _Chars = new DbFunctions<_Char>(_DbContext);
            _CharTrijobs = new DbFunctions<_CharTrijob>(_DbContext);
            _TrainingCampMembers = new DbFunctions<_TrainingCampMember>(_DbContext);
            _GuildMembers = new DbFunctions<_GuildMember>(_DbContext);
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
