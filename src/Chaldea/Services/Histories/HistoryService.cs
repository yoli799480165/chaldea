using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Chaldea.Core.Repositories;
using Chaldea.Exceptions;
using Chaldea.Services.Histories.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Chaldea.Services.Histories
{
    [Authorize]
    [Route("api/history")]
    public class HistoryService : ServiceBase
    {
        private readonly IRepository<string, History> _historyRepository;
        private readonly ILogger<HistoryService> _logger;

        public HistoryService(
            ILogger<HistoryService> logger,
            IRepository<string, History> historyRepository
        )
        {
            _logger = logger;
            _historyRepository = historyRepository;
        }

        private string HistoryPath
        {
            get
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "statics", "imgs", "history");
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                return path;
            }
        }

        [Route("addOrUpdateHistory")]
        [HttpPost]
        public async Task AddOrUpdateHistory([FromBody] HistoryDto input)
        {
            if (input == null) throw new UserFriendlyException($"Invalid parameter {nameof(input)}");

            try
            {
                var history =
                    await _historyRepository.GetAsync(x => x.UserId == UserId && x.ResourceId == input.ResourceId);
                if (history == null)
                {
                    var id = Guid.NewGuid().ToString("N");
                    var screenshotFile = $"{id}.jpg";
                    System.IO.File.WriteAllBytes(Path.Combine(HistoryPath, screenshotFile),
                        GetDataFromBase64Image(input.Screenshot));
                    var entity = Mapper.Map<History>(input);
                    entity.Id = id;
                    entity.UserId = UserId;
                    entity.CreationTime = DateTime.UtcNow;
                    entity.LastModificationTime = DateTime.UtcNow;
                    entity.Screenshot = screenshotFile;
                    await _historyRepository.AddAsync(entity);
                }
                else
                {
                    var screenshotFile = $"{Guid.NewGuid():N}.jpg";
                    System.IO.File.WriteAllBytes(Path.Combine(HistoryPath, screenshotFile),
                        GetDataFromBase64Image(input.Screenshot));
                    history.LastModificationTime = DateTime.UtcNow;
                    history.CurrentTime = input.CurrentTime;
                    history.Screenshot = screenshotFile;
                    await _historyRepository.UpdateAsync(history);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new UserFriendlyException("Add history failed.");
            }
        }

        [Route("getHistories")]
        [HttpGet]
        public async Task<ICollection<HistoryDetailDto>> GetHistories(int skip, int take)
        {
            try
            {
                var stages = new List<string>
                {
                    "{$match:{userId:'" + UserId + "'}}",
                    "{$lookup:{localField:'animeId',from:'animes',foreignField:'_id',as:'anime'}}",
                    "{$unwind:'$anime'}",
                    "{$project:{'_id':1,'lastModificationTime':1,'currentTime':1,'screenshot':1,'animeId':1,'animeTitle':'$anime.title','sourceTitle':1,'duration':1}}",
                    "{$sort:{'lastModificationTime':-1}}"
                };

                if (skip >= 0 && take > 0)
                {
                    stages.Add("{$skip:" + skip + "}");
                    stages.Add("{$limit:" + take + "}");
                }

                var pipeline = PipelineDefinition<History, HistoryDetailDto>.Create(stages);
                var histories = await _historyRepository.Aggregate(pipeline).ToListAsync();
                return histories;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new UserFriendlyException("Get histories failed.");
            }
        }

        [Route("getAnimeHistories")]
        [HttpGet]
        public async Task<ICollection<HistoryDto>> GetAnimeHistories(string animeId)
        {
            if (string.IsNullOrEmpty(animeId)) throw new UserFriendlyException($"Invalid parameter {nameof(animeId)}");

            try
            {
                var sort = Builders<History>.Sort.Ascending(x => x.LastModificationTime);
                var history = await _historyRepository
                    .GetAll(x => x.UserId == UserId && x.AnimeId == animeId)
                    .Sort(sort)
                    .ToListAsync();

                var data = Mapper.Map<ICollection<HistoryDto>>(history);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new UserFriendlyException("Get anime histories failed.");
            }
        }

        private byte[] GetDataFromBase64Image(string base64Image)
        {
            var base64 = base64Image.Remove(0, 23);
            var data = Convert.FromBase64String(base64);
            return data;
        }
    }
}