using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Chaldea.Repositories;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace Chaldea.Services
{
    public class MigrateService : ServiceBase
    {
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