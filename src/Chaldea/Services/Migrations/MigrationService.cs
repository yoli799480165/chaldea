using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chaldea.Exceptions;
using Chaldea.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Chaldea.Services.Migrations
{
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

    public class Data
    {
        public string Name { get; set; }

        public List<DataItem> Animes { get; set; }
    }

    public class DataItem
    {
        public string Title { get; set; }

        public string ImgName { get; set; }

        public string Desc { get; set; }
    }
}


//        private readonly IRepository<string, AnimeDetail> _animeDetailRepository;
//        private readonly IRepository<string, Bangumi> _bangumiRepository;
//        private readonly string _baseDir = Directory.GetCurrentDirectory();
//
//        public MigrateService(
//            IRepository<string, Bangumi> bangumiRepository,
//            IRepository<string, AnimeDetail> animeDetailRepository
//        )
//        {
//            _bangumiRepository = bangumiRepository;
//            _animeDetailRepository = animeDetailRepository;
//        }
//
//        [Route("api/migrate")]
//        [HttpGet]
//        public async Task Migrate(string fileName)
//        {
//            var dataPath = Path.Combine(_baseDir, "statics", $"{fileName}.json");
//
//            if (System.IO.File.Exists(dataPath))
//            {
//                Console.WriteLine($"还原的文件路径为：{dataPath}");
//                var json = System.IO.File.ReadAllText(dataPath);
//                var dataList = JsonConvert.DeserializeObject<List<Data>>(json);
//
//                Console.WriteLine("正在提取数据...");
//                var bangumiList = new List<Bangumi>();
//                var animeDetailList = new List<AnimeDetail>();
//
//                foreach (var data in dataList)
//                {
//                    var bangumi = new Bangumi
//                    {
//                        Id = ObjectId.GenerateNewId().ToString(),
//                        Name = data.Name,
//                        Animes = new List<Anime>()
//                    };
//
//                    foreach (var item in data.Animes)
//                    {
//                        // 创建Anime简单信息
//                        var anime = new Anime
//                        {
//                            Id = ObjectId.GenerateNewId().ToString(),
//                            Cover = item.ImgName,
//                            Title = item.Title
//                        };
//
//                        bangumi.Animes.Add(anime);
//
//                        // 创建Anime详细信息
//                        var animeDetail = new AnimeDetail
//                        {
//                            Id = ObjectId.GenerateNewId().ToString(),
//                            AnimeId = anime.Id,
//                            Desc = item.Desc.Replace("简介：\n", "").Trim(),
//                            Animes = new List<Resource>(),
//                            Comics = new List<Resource>(),
//                            Novels = new List<Resource>(),
//                            Comments = new List<Comment>()
//                        };
//                        animeDetailList.Add(animeDetail);
//                    }
//
//                    bangumiList.Add(bangumi);
//                }
//
//                Console.WriteLine($"数据读取完毕，共计: {dataList.Count}条.");
//
//                Console.WriteLine($"开始还原数据...");
//                await _bangumiRepository.AddManyAsync(bangumiList);
//                await _animeDetailRepository.AddManyAsync(animeDetailList);
//                Console.WriteLine("数据还原结束.");
//            }
//        }

//        [Route("api/migrateTags")]
//        [HttpGet]
//        public async Task MigrateTags()
//        {
//            var list = new List<Tag>();
//            list.Add(new Tag { Name = });
//        }