using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Chaldea.Repositories;
using Chaldea.Services.Bangumis.Dto;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Chaldea.Services.Bangumis
{
    [Route("api/bangumi")]
    public class BangumiService : ServiceBase
    {
        private readonly IRepository<string, Bangumi> _bangumiRepository;

        public BangumiService(IRepository<string, Bangumi> bangumiRepository)
        {
            _bangumiRepository = bangumiRepository;
        }

        [Route("create")]
        [HttpPut]
        public async Task Create([FromBody] Bangumi bangumi)
        {
            if (bangumi == null)
                throw new Exception("");

            if (string.IsNullOrEmpty(bangumi.Name))
                throw new Exception("");

            await _bangumiRepository.AddAsync(bangumi);
        }

        [Route("getList")]
        [HttpGet]
        public async Task<ICollection<Bangumi>> GetList(int skip, int take)
        {
            if (skip == 0 && take == 0)
                return await _bangumiRepository
                    .GetAll()
                    .SortByDescending(x => x.Name)
                    .ToListAsync();

            var query = Builders<Bangumi>.Projection.Slice(x => x.Animes, 0, 6);
            var list = await _bangumiRepository
                .GetAll()
                .SortByDescending(x => x.Name)
                .Project<Bangumi>(query)
                .Skip(skip)
                .Limit(take)
                .ToListAsync();
            return list;
        }

        [Route("import")]
        [HttpPost]
        public void Import([FromBody] ImportBangumiDto input)
        {
            var imgPath = Path.Combine(Directory.GetCurrentDirectory(), "statics", "imgs");
            if (!Directory.Exists(imgPath))
                Directory.CreateDirectory(imgPath);

            var webClient = new WebClient();
            var bangumiName = Path.GetFileNameWithoutExtension(input.Url).Substring(2).Insert(4, "-");
            var bangumi = new Bangumi
            {
                Name = bangumiName,
                Animes = new List<Anime>()
            };
            var web = new HtmlWeb();
            var baseUrl = new Uri(input.Url).Host;
            var animeListPage = web.Load(input.Url);
            var animeListNodes = animeListPage.DocumentNode.SelectNodes("//div[@class='zt-dh adj2']/ul/li/p/a/img");
            var animeDetailUrls = new Dictionary<string, string>();
            foreach (var animeListNode in animeListNodes)
            {
                var href = animeListNode.ParentNode.Attributes["href"].Value;
                animeDetailUrls.TryAdd(href, $"{baseUrl}{href}");
            }

            foreach (var animeDetailUrl in animeDetailUrls)
            {
                var animeDetailPage = web.Load(animeDetailUrl.Value);
                // title
                var title = animeDetailPage.DocumentNode.SelectSingleNode("//div[@class='anime-img']/h1").InnerText
                    .Trim();

                // image
                var imgUrl = animeDetailPage.DocumentNode.SelectSingleNode("//div[@class='anime-img']/img")
                    .Attributes["src"].Value;

                var downloadUrl = imgUrl.Substring(0, 4).ToLower() == "http" ? imgUrl : $"{baseUrl}{imgUrl}";
                var imgName = Path.GetFileName(imgUrl);
                var savePath = $"{imgPath}\\{imgName}";
                if (!System.IO.File.Exists(savePath))
                    webClient.DownloadFile(downloadUrl, savePath);

                // desc
                var desc = "";
                var node = animeDetailPage
                    .DocumentNode.SelectSingleNode("//div[@id='box']/span[@id='showall']");
                if (node == null)
                {
                    node = animeDetailPage
                        .DocumentNode.SelectSingleNode("//div[@id='box']");
                    desc = node.InnerText.Trim();
                }
                else
                {
                    desc = node.InnerText.Trim();
                }

                var anime = new Anime
                {
                    Title = title,
                    Cover = imgName,
                    Desc = desc
                };
                bangumi.Animes.Add(anime);
            }
        }

        [Route("{id}/export")]
        [HttpPost]
        public async Task<Bangumi> Export(string id)
        {
            return new Bangumi();
        }
    }
}