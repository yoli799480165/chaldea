using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Chaldea.Core.Repositories;
using Chaldea.Exceptions;
using Chaldea.Services.Histories.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Chaldea.Services.Histories
{
    [Authorize]
    [Route("api/history")]
    public class HistoryService : ServiceBase
    {
        private readonly IRepository<string, History> _historyRepository;

        public HistoryService(IRepository<string, History> historyRepository)
        {
            _historyRepository = historyRepository;
        }

        [Route("addOrUpdateHistory")]
        [HttpPost]
        public async Task AddOrUpdateHistory([FromBody] HistoryDto input)
        {
            if (input == null) throw new UserFriendlyException($"Invalid parameter {nameof(input)}");

            var history =
                await _historyRepository.GetAsync(x => x.UserId == UserId && x.ResourceId == input.ResourceId);
            if (history == null)
            {
                var entity = Mapper.Map<History>(input);
                entity.Id = Guid.NewGuid().ToString("N");
                entity.UserId = UserId;
                entity.CreationTime = DateTime.UtcNow;
                entity.LastModificationTime = DateTime.UtcNow;
                await _historyRepository.AddAsync(entity);
            }
            else
            {
                history.LastModificationTime = DateTime.UtcNow;
                history.CurrentTime = input.CurrentTime;
                await _historyRepository.UpdateAsync(history);
            }
        }

        [Route("getAnimeHistories")]
        [HttpGet]
        public async Task<ICollection<HistoryDto>> GetAnimeHistories(string animeId)
        {
            var sort = Builders<History>.Sort.Ascending(x => x.LastModificationTime);
            var history = await _historyRepository
                .GetAll(x => x.UserId == UserId && x.AnimeId == animeId)
                .Sort(sort)
                .ToListAsync();

            var data = Mapper.Map<ICollection<HistoryDto>>(history);
            return data;
        }
    }
}