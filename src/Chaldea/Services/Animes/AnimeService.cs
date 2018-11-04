using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Chaldea.Core.Repositories;
using Chaldea.Exceptions;
using Chaldea.Services.Animes.Dto;
using Chaldea.Services.Bangumis.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Chaldea.Services.Animes
{
    [Route("api/anime")]
    public class AnimeService : ServiceBase
    {
        private readonly IRepository<string, Anime> _animeRepository;

        private readonly IRepository<string, Bangumi> _bangumiRepository;
        private readonly ILogger<AnimeService> _logger;
        private readonly IRepository<string, Video> _videoRepository;

        public AnimeService(
            ILogger<AnimeService> logger,
            IRepository<string, Anime> animeRepository,
            IRepository<string, Bangumi> bangumiRepository,
            IRepository<string, Video> videoRepository)
        {
            _logger = logger;
            _animeRepository = animeRepository;
            _bangumiRepository = bangumiRepository;
            _videoRepository = videoRepository;
        }

        [Route("getList")]
        [HttpGet]
        public async Task<ICollection<AnimeOutlineDto>> GetList(string bangumiId, int skip, int take)
        {
            try
            {
                if (string.IsNullOrEmpty(bangumiId))
                {
                    var projection = Builders<Anime>.Projection.Include(x => x.Id).Include(x => x.Title)
                        .Include(x => x.Cover);
                    ICollection<AnimeOutlineDto> list;
                    if (skip >= 0 && take > 0)
                        list = await _animeRepository.GetAll().Skip(skip).Limit(take)
                            .Project<AnimeOutlineDto>(projection).ToListAsync();
                    else
                        list = await _animeRepository.GetAll().Project<AnimeOutlineDto>(projection).ToListAsync();

                    return list;
                }

                var stages = new List<string>
                {
                    "{$match:{_id:'" + bangumiId + "'}}",
                    "{$lookup:{localField:'animes',from:'animes',foreignField:'_id',as:'animes'}}",
                    "{$project:{'animes._id':1,'animes.title':1,'animes.cover':1}}"
                };

                if (skip >= 0 && take > 0)
                    stages.Add("{$project:{animes:{$slice:['$animes'," + skip + "," + take + "]}}}");
                var pipeline = PipelineDefinition<Bangumi, BangumiAnimesDto>.Create(stages);
                var bangumi = await _bangumiRepository.Aggregate(pipeline).FirstOrDefaultAsync();
                return bangumi.Animes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new UserFriendlyException("Get anime list failed.");
            }
        }

        [Route("getAnime")]
        [HttpGet]
        public async Task<AnimeDto> GetAnime(string animeId)
        {
            if (string.IsNullOrEmpty(animeId))
                throw new UserFriendlyException($"Invalid parameter {nameof(animeId)}");

            try
            {
                var anime = await _animeRepository.GetAsync(x => x.Id == animeId);
                return Mapper.Map<AnimeDto>(anime);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new UserFriendlyException("Get anime failed.");
            }
        }

        [Route("update")]
        [HttpPost]
        public async Task UpdateAnime([FromBody] AnimeDto input)
        {
            try
            {
                if (input == null)
                    throw new UserFriendlyException($"Invalid parameter {nameof(input)}");

                if (string.IsNullOrEmpty(input.Id))
                    throw new UserFriendlyException($"{input.Id} can not be null.");

                var anime = Mapper.Map<Anime>(input);
                await _animeRepository.UpdateAsync(anime);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Route("{id}/removeVideos")]
        [HttpDelete]
        public async Task RemoveVideos(string id, [FromBody] ICollection<string> resources)
        {
            var filter = Builders<Anime>.Filter.Eq(x => x.Id, id);
            var update = Builders<Anime>.Update.PullFilter(x => x.Videos, y => resources.Contains(y.Id));
            await _animeRepository.UpdateAsync(filter, update);
//            var delete = Builders<Video>.Filter.In(x => x.Id, resources);
//            await _videoRepository.DeleteManyAsync(delete);
        }
    }
}