using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chaldea.Repositories;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Chaldea.Services
{
    [Route("api/anime")]
    public class AnimeService : ServiceBase
    {
        private readonly IRepository<ObjectId, AnimeDetail> _animeDetailRepository;
        private readonly IRepository<ObjectId, Bangumi> _bangumiRepository;

        public AnimeService(
            IRepository<ObjectId, Bangumi> bangumiRepository,
            IRepository<ObjectId, AnimeDetail> animeDetailRepository)
        {
            _bangumiRepository = bangumiRepository;
            _animeDetailRepository = animeDetailRepository;
        }

        [Route("getlist")]
        [HttpGet]
        public async Task<ICollection<Anime>> GetList(string bangumiId, int skip, int take)
        {
            var id = RepositoryHelper.GetInternalId(bangumiId);

            var query = Builders<Bangumi>.Projection.Slice(x => x.Animes, skip, take);
            var bangumi = await _bangumiRepository
                .GetAll(x => x.Id == id)
                .Project<Bangumi>(query).FirstOrDefaultAsync();
            return bangumi.Animes;
        }

        [Route("getdetail")]
        [HttpGet]
        public async Task<AnimeDetail> GetDetail(string animeId)
        {
            return await _animeDetailRepository.GetAsync(x => x.AnimeId == animeId);
        }

        [Route("addresource")]
        [HttpPut]
        public async Task AddResource([FromBody] AddResourceDto input)
        {
            foreach (var source in input.Resources)
            {
                source.Uid = Guid.NewGuid().ToString("N");
                source.Id = ObjectId.GenerateNewId();
            }

            var filter = Builders<AnimeDetail>
                .Filter.Eq(e => e.AnimeId, input.AnimeId);
            var update = input.SourceType == SourceType.Anime
                ? Builders<AnimeDetail>.Update.PushEach(x => x.Animes, input.Resources)
                : input.SourceType == SourceType.Comic
                    ? Builders<AnimeDetail>.Update.PushEach(x => x.Comics, input.Resources)
                    : Builders<AnimeDetail>.Update.PushEach(x => x.Novels, input.Resources);

            await _animeDetailRepository.UpdateAsync(filter, update);
        }
    }

    public class AddResourceDto
    {
        public string AnimeId { get; set; }

        public SourceType SourceType { get; set; }

        public List<Resource> Resources { get; set; }
    }

    public enum SourceType
    {
        Anime = 0,
        Comic = 1,
        Novel = 2
    }
}