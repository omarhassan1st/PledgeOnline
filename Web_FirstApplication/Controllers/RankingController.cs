using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_FirstApplication.Models.DbModel.Shard;
using Web_FirstApplication.Repository.Declaration.IShard;

namespace Web_FirstApplication.Controllers
{
    [AllowAnonymous]
    public class RankingController : Controller
    {
        private readonly IShardDbContext _ShardDbContext;
        public RankingController(IShardDbContext shardDbContext)
        {
            _ShardDbContext = shardDbContext;
        }
        [HttpGet]
        [ActionName("TopPlayer")]
        public IActionResult OnGetTopPlayer()
        {
            IEnumerable<_Char> Users = _ShardDbContext._Chars.Take(10).ToList();
            foreach (_Char _char in Users)
            {
                //Guild Name
                _char.NickName16 =
                    _ShardDbContext._Guilds.Find(g => g.ID == (_char.GuildID ?? 0)).Name;
            }
            return View(Users);
        }

        [HttpPost]
        [ActionName("TopPlayer")]
        [ValidateAntiForgeryToken]
        public IActionResult OnPostTopPlayer(string CharName)
        {
            if (!string.IsNullOrEmpty(CharName))
            {
                IEnumerable<_Char> Users = _ShardDbContext._Chars
                    .Where(u => u.CharName16.Contains(CharName)).Take(10).ToList();
                foreach (_Char _char in Users)
                {
                    //Guild Name
                    _char.NickName16 = _ShardDbContext._Guilds.Find(g => g.ID == (_char.GuildID ?? 0)).Name;
                }
                TempData["CharName"] = CharName;
                return View(Users);
            }
            return View(new List<_Char> { });
        }

        [HttpGet]
        [ActionName("TopGuild")]
        public IActionResult OnGetTopGuild()
        {
            IEnumerable<_Guild> guilds = _ShardDbContext._Guilds.Take(10).ToList();
            return View(guilds);
        }

        [HttpPost]
        [ActionName("TopGuild")]
        [ValidateAntiForgeryToken]
        public IActionResult OnPostTopGuild(string guildName)
        {
            if (!string.IsNullOrEmpty(guildName))
            {
                IEnumerable<_Guild> guilds = _ShardDbContext
                    ._Guilds.Where(g => g.Name.Contains(guildName)).Take(10).ToList();
                TempData["guildName"] = guildName;
                return View(guilds);

            }
            return View(new List<_Guild> { });
        }

        [HttpGet]
        [ActionName("TopThieves")]
        public IActionResult OnGetTopThieves()
        {
            IEnumerable<_CharTrijob> _CharTrijobs = _ShardDbContext._CharTrijobs
                .Where(Ct => Ct.JobType == 1).Take(10).ToList();
            foreach (_CharTrijob Char in _CharTrijobs)
            {
                Char.CharName = _ShardDbContext._Chars.Find(Cn => Cn.CharID == Char.CharID).CharName16;
            }
            return View(_CharTrijobs);
        }

        [HttpPost]
        [ActionName("TopThieves")]
        [ValidateAntiForgeryToken]
        public IActionResult OnPostTopThieves(string ThiveName)
        {
            if (!string.IsNullOrEmpty(ThiveName))
            {
                IEnumerable<_CharTrijob> _CharTrijobsReady = _ShardDbContext._CharTrijobs
                    .Where(Ct => Ct.JobType == 1).Take(10).ToList();
                foreach (_CharTrijob Char in _CharTrijobsReady)
                {
                    Char.CharName = _ShardDbContext._Chars.Find(Cn => Cn.CharID == Char.CharID).CharName16;
                }
                IEnumerable<_CharTrijob> _CharTrijobs = _CharTrijobsReady
                    .Where(Ct => Ct.CharName.Contains(ThiveName)).Take(10).ToList();
                TempData["ThiveName"] = ThiveName;
                return View(_CharTrijobs);
            }
            return View(new List<_CharTrijob> { });
        }

        [HttpGet]
        [ActionName("TopHunter")]
        public IActionResult OnGetTopHunter()
        {
            IEnumerable<_CharTrijob> _CharTrijobs = _ShardDbContext._CharTrijobs
                .Where(Ct => Ct.JobType == 2).Take(10).ToList();
            foreach (_CharTrijob Char in _CharTrijobs)
            {
                Char.CharName = _ShardDbContext._Chars.Find(Cn => Cn.CharID == Char.CharID).CharName16;
            }
            return View(_CharTrijobs);
        }

        [HttpPost]
        [ActionName("TopHunter")]
        [ValidateAntiForgeryToken]
        public IActionResult OnPostTopHunter(string HunterName)
        {
            if (!string.IsNullOrEmpty(HunterName))
            {
                IEnumerable<_CharTrijob> _CharTrijobsReady = _ShardDbContext._CharTrijobs
                    .Where(Ct => Ct.JobType == 2).Take(10).ToList();
                foreach (_CharTrijob Char in _CharTrijobsReady)
                {
                    Char.CharName = _ShardDbContext._Chars.Find(Cn => Cn.CharID == Char.CharID).CharName16;
                }
                IEnumerable<_CharTrijob> _CharTrijobs = _CharTrijobsReady
                    .Where(Ct => Ct.CharName.Contains(HunterName)).Take(10).ToList();
                TempData["HunterName"] = HunterName;
                return View(_CharTrijobs);
            }
            return View(new List<_CharTrijob> { });
        }

        [HttpGet]
        [ActionName("TopTreader")]
        public IActionResult OnGetTopTreader()
        {
            IEnumerable<_CharTrijob> _CharTrijobs = _ShardDbContext._CharTrijobs
                .Where(Ct => Ct.JobType == 3).Take(10).ToList();
            foreach (_CharTrijob Char in _CharTrijobs)
            {
                Char.CharName = _ShardDbContext._Chars.Find(Cn => Cn.CharID == Char.CharID).CharName16;
            }
            return View(_CharTrijobs);
        }

        [HttpPost]
        [ActionName("TopTreader")]
        [ValidateAntiForgeryToken]
        public IActionResult OnPostTopTreader(string TreaderName)
        {
            if (!string.IsNullOrEmpty(TreaderName))
            {
                IEnumerable<_CharTrijob> _CharTrijobsReady = _ShardDbContext._CharTrijobs
                    .Where(Ct => Ct.JobType == 3).Take(10).ToList();
                foreach (_CharTrijob Char in _CharTrijobsReady)
                {
                    Char.CharName = _ShardDbContext._Chars.Find(Cn => Cn.CharID == Char.CharID).CharName16;
                }
                IEnumerable<_CharTrijob> _CharTrijobs = _CharTrijobsReady
                    .Where(Ct => Ct.CharName.Contains(TreaderName)).Take(10).ToList();
                TempData["TreaderName"] = TreaderName;
                return View(_CharTrijobs);
            }
            return View(new List<_CharTrijob> { });
        }

        [HttpGet]
        [ActionName("TopHonor")]
        public IActionResult OnGetTopHonor()
        {
            IEnumerable<_TrainingCampMember> _TrainingCampMembers = _ShardDbContext._TrainingCampMembers.Take(10)
                .OrderByDescending(h => h.HonorPoint).ToList();
            foreach (_TrainingCampMember Char in _TrainingCampMembers)
            {
                Char.GuildName = _ShardDbContext._Guilds.Find(
                        gn => gn.ID == _ShardDbContext._GuildMembers.Find(
                        g => g.CharName == Char.CharName).GuildID).Name;
            }
            return View(_TrainingCampMembers);
        }

        [HttpPost]
        [ActionName("TopHonor")]
        [ValidateAntiForgeryToken]
        public IActionResult OnPostHonor(string CharName)
        {
            if (!string.IsNullOrEmpty(CharName))
            {
                IEnumerable<_TrainingCampMember> _TrainingCampMembers = _ShardDbContext._TrainingCampMembers
                           .Where(u => u.CharName.Contains(CharName)).Take(10)
                           .OrderByDescending(h => h.HonorPoint).ToList();
                foreach (_TrainingCampMember Char in _TrainingCampMembers)
                {
                    Char.GuildName = _ShardDbContext._Guilds.Find(
                            gn => gn.ID == _ShardDbContext._GuildMembers.Find(
                            g => g.CharName == Char.CharName).GuildID).Name;
                }
                TempData["CharName"] = CharName;
                return View(_TrainingCampMembers);
            }
            return View(new List<_TrainingCampMember> { });
        }
    }
}
