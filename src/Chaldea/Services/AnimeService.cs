using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chaldea.Repositories;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

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

            if (skip == 0 && take == 0) return (await _bangumiRepository.GetAsync(id)).Animes;
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
            foreach (var source in input.Resources) source.Uid = Guid.NewGuid().ToString("N");

            var filter = Builders<AnimeDetail>
                .Filter.Eq(e => e.AnimeId, input.AnimeId);
            var update = input.SourceType == SourceType.Anime
                ? Builders<AnimeDetail>.Update.PushEach(x => x.Animes, input.Resources)
                : input.SourceType == SourceType.Comic
                    ? Builders<AnimeDetail>.Update.PushEach(x => x.Comics, input.Resources)
                    : Builders<AnimeDetail>.Update.PushEach(x => x.Novels, input.Resources);

            await _animeDetailRepository.UpdateAsync(filter, update);
        }

        [Route("updateDetail")]
        [HttpPost]
        public async Task UpdateDetail([FromBody] AnimeDetail input)
        {
            if (input == null) throw new Exception($"Invalid param {nameof(input)}");
            if (_animeDetailRepository.GetAsync(input.Id) == null)
                throw new Exception($"The id {input.Id} not exists.");
            foreach (var anime in input.Animes)
                if (string.IsNullOrEmpty(anime.Uid))
                    anime.Uid = Guid.NewGuid().ToString("N");

            await _animeDetailRepository.UpdateAsync(input);
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

    public class ObjectIdConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ObjectId);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.String)
                throw new Exception($"Unexpected token parsing ObjectId. Expected String, got {reader.TokenType}.");

            var value = (string) reader.Value;
            return string.IsNullOrEmpty(value) ? ObjectId.Empty : new ObjectId(value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is ObjectId)
            {
                var objectId = (ObjectId) value;
                writer.WriteValue(objectId != ObjectId.Empty ? objectId.ToString() : string.Empty);
            }
            else
            {
                throw new Exception("Expected ObjectId value.");
            }
        }
    }
}