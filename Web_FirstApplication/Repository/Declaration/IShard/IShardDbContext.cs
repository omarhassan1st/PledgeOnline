using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_FirstApplication.Models.DbModel.Shard;
using Web_FirstApplication.Repository.Declaration.Shared;

namespace Web_FirstApplication.Repository.Declaration.IShard
{
    public interface IShardDbContext
    {
        IDbFunctions<_Guild> _Guilds { get; set; }
        IDbFunctions<_SiegeFortress> _SiegeFortress { get; set; }
        IDbFunctions<_Char> _Chars { get; set; }
        IDbFunctions<_CharTrijob> _CharTrijobs { get; set; }
        IDbFunctions<_TrainingCampMember> _TrainingCampMembers { get; set; }
        IDbFunctions<_GuildMember> _GuildMembers { get; set; }

        public int Complete();
    }
}
