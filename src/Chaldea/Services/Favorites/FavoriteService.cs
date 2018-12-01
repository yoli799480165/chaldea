using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chaldea.Core.Repositories;
using Chaldea.Exceptions;
using Chaldea.Services.Favorites.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Chaldea.Services.Favorites
{
    [Authorize]
    [Route("api/favorite")]
    public class FavoriteService : ServiceBase
    {
        private readonly IRepository<string, Anime> _animeRepository;
        private readonly IRepository<string, Favorite> _favoriteRepository;
        private readonly ILogger<FavoriteService> _logger;

        public FavoriteService(
            IRepository<string, Favorite> favoriteRepository,
            IRepository<string, Anime> animeRepository,
            ILogger<FavoriteService> logger)
        {
            _favoriteRepository = favoriteRepository;
            _animeRepository = animeRepository;
            _logger = logger;
        }

        [HttpPost]
        [Route("subscribe")]
        public async Task Subscribe(string animeId)
        {
            if (string.IsNullOrEmpty(animeId)) throw new UserFriendlyException($"Invalid parameter {nameof(animeId)}");

            try
            {
                var count = await _favoriteRepository.CountAsync(x => x.UserId == UserId && x.AnimeId == animeId);
                if (count <= 0)
                {
                    var input = new Favorite
                    {
                        AnimeId = animeId,
                        CreationTime = DateTime.UtcNow,
                        UserId = UserId,
                        Id = Guid.NewGuid().ToString("N")
                    };
                    await _favoriteRepository.AddAsync(input);

                    var filter = Builders<Anime>.Filter.Eq("_id", animeId);
                    var update = Builders<Anime>.Update.Inc(x => x.SubCounts, 1);
                    await _animeRepository.UpdateAsync(filter, update);
                }
                else
                {
                    throw new Exception($"Anime {animeId} already subscribed.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new UserFriendlyException("Subscribe anime failed.");
            }
        }

        [HttpPost]
        [Route("unSubscribe")]
        public async Task UnSubscribe(string animeId)
        {
            if (string.IsNullOrEmpty(animeId)) throw new UserFriendlyException($"Invalid parameter {nameof(animeId)}");
            var favorite = await _favoriteRepository.GetAsync(x => x.UserId == UserId && x.AnimeId == animeId);
            if (favorite == null)
            {
                _logger.LogError($"Anime {animeId} not subscribed.");
                throw new UserFriendlyException($"Anime {animeId} not subscribed.");
            }

            try
            {
                await _favoriteRepository.DeleteAsync(favorite.Id);
                var filter = Builders<Anime>.Filter.Eq("_id", animeId);
                var update = Builders<Anime>.Update.Inc(x => x.SubCounts, -1);
                await _animeRepository.UpdateAsync(filter, update);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new UserFriendlyException("UnSubscribe anime failed.");
            }
        }

        [Route("getFavorites")]
        [HttpGet]
        public async Task<ICollection<FavoriteDto>> GetFavorites(int skip, int take)
        {
            try
            {
                var stages = new List<string>
                {
                    "{$match: {'userId':'" + UserId + "'}}",
                    "{$sort:{'creationTime':-1}}",
                    "{$lookup:{from:'historys',let:{fav_userId:'$userId',fav_animeId:'$animeId'},pipeline:[{$match:{$expr:{$and:[{$eq:['$userId','$$fav_userId']},{$eq:['$animeId','$$fav_animeId']}]}}}],as:'histories'}}",
                    "{$lookup:{from:'animes',localField:'animeId',foreignField:'_id',as:'anime'}}",
                    "{$unwind:'$anime'}",
                    "{$project:{'_id':1,'animeId':1,'title':'$anime.title','cover':'$anime.cover','videos':'$anime.videos','histories.sourceTitle':1}}",
                    "{$project:{'_id':1,'animeId':1,'title':1,'cover':1,'video':{$arrayElemAt:['$videos',-1]},'history':{$arrayElemAt:['$histories',-1]}}}",
                    "{$project:{'_id':1,'animeId':1,'title':1,'cover':1,'lastUpdate':'$video.name','currentPlay':'$history.sourceTitle'}}"
                };

                if (skip >= 0 && take > 0)
                    stages.InsertRange(1, new[]
                    {
                        "{$skip:" + skip + "}",
                        "{$limit:" + take + "}"
                    });

                var pipeline = PipelineDefinition<Favorite, FavoriteDto>.Create(stages);
                var favorites = await _favoriteRepository.Aggregate(pipeline).ToListAsync();
                return favorites;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new UserFriendlyException("Get favorites failed.");
            }
        }
    }
}