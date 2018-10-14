﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Chaldea.Exceptions;
using Chaldea.Repositories;
using Chaldea.Services.Bangumis.Dto;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Chaldea.Services.Bangumis
{
    [Route("api/bangumi")]
    public class BangumiService : ServiceBase
    {
        private readonly IRepository<string, Anime> _animeRepository;
        private readonly IRepository<string, Bangumi> _bangumiRepository;
        private readonly ILogger<BangumiService> _logger;

        public BangumiService(
            ILogger<BangumiService> logger,
            IRepository<string, Bangumi> bangumiRepository,
            IRepository<string, Anime> animeRepository
        )
        {
            _logger = logger;
            _bangumiRepository = bangumiRepository;
            _animeRepository = animeRepository;
        }

        [Route("createOrUpdate")]
        [HttpPut]
        public async Task Create([FromBody] BangumiEditDto input)
        {
            if (input == null)
                throw new UserFriendlyException($"Invalid parameter {nameof(input)}");

            if (string.IsNullOrEmpty(input.Name))
                throw new UserFriendlyException($"Invalid parameter {nameof(input.Name)}");

            try
            {
                if (string.IsNullOrEmpty(input.Id))
                {
                    var bangumi = Mapper.Map<Bangumi>(input);
                    bangumi.Id = Guid.NewGuid().ToString("N");
                    bangumi.Animes = new List<string>();
                    await _bangumiRepository.AddAsync(bangumi);
                }
                else
                {
                    var filter = Builders<Bangumi>.Filter.Eq("_id", input.Id);
                    var update = Builders<Bangumi>.Update.Set("name", input.Name);
                    await _bangumiRepository.UpdateAsync(filter, update);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new UserFriendlyException("Create bangumi failed.");
            }
        }

        [Route("{id}/delete")]
        [HttpDelete]
        public async Task Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new UserFriendlyException($"Invalid parameter {nameof(id)}");

            try
            {
                await _bangumiRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new UserFriendlyException("Delete bangumi failed.");
            }
        }

        [Route("getList")]
        [HttpGet]
        public async Task<ICollection<BangumiDto>> GetList()
        {
            try
            {
                var query = Builders<Bangumi>.Projection.Include(x => x.Id).Include(x => x.Name);
                var list = await _bangumiRepository.GetAll().Project<Bangumi>(query).ToListAsync();
                return Mapper.Map<List<BangumiDto>>(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new UserFriendlyException("Get bangumi list failed.");
            }
        }

        [Route("getAnimes")]
        [HttpGet]
        public async Task<ICollection<BangumiAnimesDto>> GetAnimes(int skip, int take, int slice)
        {
            try
            {
                var stages = new List<string>
                {
                    "{$lookup:{localField:'animes',from:'animes',foreignField:'_id',as:'animes'}}",
                    "{$project:{'_id':1,'name':1,'animes._id':1,'animes.title':1,'animes.cover':1}}"
                };

                if (slice > 0) stages.Add("{$project:{'_id':1,'name':1,animes:{$slice:['$animes'," + slice + "]}}}");

                if (skip >= 0 && take > 0)
                {
                    stages.Add("{$skip: " + skip + "}");
                    stages.Add("{$limit: " + take + "}");
                }

                var pipeline = PipelineDefinition<Bangumi, BangumiAnimesDto>.Create(stages);

                var list = await _bangumiRepository.Aggregate(pipeline).ToListAsync();
                return list;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new UserFriendlyException("Get bangumi animes failed.");
            }
        }

        [Route("{id}/import")]
        [HttpPost]
        public async Task Import(string id, [FromBody] ImportBangumiDto input)
        {
            if (string.IsNullOrEmpty(id))
                throw new UserFriendlyException($"Invalid parameter {nameof(id)}");

            if (input == null)
                throw new UserFriendlyException($"Invalid parameter {nameof(input)}");

            if (string.IsNullOrEmpty(input.Url))
                throw new UserFriendlyException($"Invalid parameter {nameof(input.Url)}");

            _logger.LogInformation($"Begin to import, begin date: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

            var imgPath = Path.Combine(Directory.GetCurrentDirectory(), "statics", "imgs");
            if (!Directory.Exists(imgPath))
                Directory.CreateDirectory(imgPath);

            var bangumi = await _bangumiRepository.GetAsync(id);
            if (bangumi == null)
                throw new Exception($"Can not find bangumi: {id}");

            if (bangumi.Animes == null) bangumi.Animes = new List<string>();
            if (input.Clear) bangumi.Animes.Clear();

            var animeList = new List<Anime>();
            var webClient = new WebClient();
            var web = new HtmlWeb();
            var baseUrl = new Uri(input.Url).GetLeftPart(UriPartial.Authority);
            var animeListPage = web.Load(input.Url);
            var animeListNodes = animeListPage.DocumentNode.SelectNodes("//div[@class='zt-dh adj2']/ul/li/p/a/img");
            var animeDetailUrls = new Dictionary<string, string>();
            foreach (var animeListNode in animeListNodes)
            {
                var href = animeListNode.ParentNode.Attributes["href"].Value;
                animeDetailUrls.TryAdd(href, $"{baseUrl}{href}");
            }

            _logger.LogInformation($"Find {animeDetailUrls.Count} resources in total.");

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
                string desc;
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
                    Id = Guid.NewGuid().ToString("N"),
                    Title = title,
                    Cover = imgName,
                    Desc = desc
                };
                animeList.Add(anime);
                bangumi.Animes.Add(anime.Id);
                _logger.LogInformation($"Add anime {anime.Id} to list.");
            }

            var filter = Builders<Bangumi>.Filter.Eq("_id", id);
            var update = Builders<Bangumi>.Update.Set("animes", bangumi.Animes);
            await _bangumiRepository.UpdateAsync(filter, update);
            await _animeRepository.AddManyAsync(animeList);
            _logger.LogInformation(
                $"Import {animeList.Count} animes successfully at {DateTime.Now:yyyy-MM-dd HH:mm:ss}.");
        }
    }
}