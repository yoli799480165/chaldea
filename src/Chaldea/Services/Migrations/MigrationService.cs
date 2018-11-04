using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chaldea.Core.Repositories;
using Chaldea.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Chaldea.Services.Migrations
{
    [Authorize(Roles = nameof(UserRoles.Admin))]
    [Route("api/migrate")]
    public class MigrationService : ServiceBase
    {
        private readonly IRepository<string, AnimeTag> _animeTagRepository;
        private readonly ILogger<MigrationService> _logger;

        public MigrationService(
            ILogger<MigrationService> logger,
            IRepository<string, AnimeTag> animeTagRepository)
        {
            _logger = logger;
            _animeTagRepository = animeTagRepository;
        }

        [Route("migrateAnimeTags")]
        [HttpGet]
        public async Task MigrateAnimeTags()
        {
            try
            {
                var animeTag = new AnimeTag
                {
                    Id = Guid.NewGuid().ToString("N"),
                    States = new List<string> {"完结", "连载"},
                    Tags = new List<string>
                    {
                        "原创",
                        "漫改",
                        "轻改",
                        "游戏改",
                        "动态漫",
                        "布袋戏",
                        "热血",
                        "奇幻",
                        "战斗",
                        "搞笑",
                        "日常",
                        "科幻",
                        "萌系",
                        "治愈",
                        "校园",
                        "少儿",
                        "泡面",
                        "恋爱",
                        "后宫",
                        "猎奇",
                        "少女",
                        "魔法",
                        "历史",
                        "机战",
                        "致郁",
                        "神魔",
                        "声控",
                        "运动",
                        "励志",
                        "音乐",
                        "推理",
                        "社团",
                        "智斗",
                        "催泪",
                        "美食",
                        "装逼",
                        "偶像",
                        "乙女",
                        "职场",
                        "古风",
                        "耽美",
                        "情色"
                    },
                    Types = new List<string>
                    {
                        "TV版",
                        "剧场版",
                        "真人版",
                        "OVA",
                        "OAD"
                    }
                };
                await _animeTagRepository.AddAsync(animeTag);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new UserFriendlyException("Migrate anime tag failed.");
            }
        }
    }
}