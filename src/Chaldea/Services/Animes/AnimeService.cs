using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Chaldea.Exceptions;
using Chaldea.Repositories;
using Chaldea.Services.Animes.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Chaldea.Services.Animes
{
    [Route("api/anime")]
    public class AnimeService : ServiceBase
    {
        private readonly IRepository<string, Bangumi> _bangumiRepository;
        private readonly ILogger<AnimeService> _logger;

        public AnimeService(
            ILogger<AnimeService> logger,
            IRepository<string, Bangumi> bangumiRepository)
        {
            _logger = logger;
            _bangumiRepository = bangumiRepository;
        }

        [Route("getList")]
        [HttpGet]
        public async Task<ICollection<AnimeOutlineDto>> GetList(string bangumiId)
        {
            try
            {
                // 获取所有bangumi下的动漫
                if (string.IsNullOrEmpty(bangumiId))
                {
                    var pipeline = PipelineDefinition<Bangumi, Bangumi>.Create(
                        "{$project:{'animes._id':1,'animes.title':1,'animes.cover':1}}",
                        "{$group:{_id:'',animes:{$addToSet:'$animes'}}}",
                        "{$addFields:{animes:{$reduce:{input:'$animes',initialValue:[],in:{$setUnion:['$$value','$$this']}}}}}");
                    var bangumi = await _bangumiRepository.Aggregate(pipeline).FirstOrDefaultAsync();
                    return Mapper.Map<ICollection<AnimeOutlineDto>>(bangumi.Animes);
                }
                // 获取制定bangumi下的动漫
                else
                {
                    var projection = Builders<Bangumi>.Projection.Include("animes._id").Include("animes.title")
                        .Include("animes.cover");
                    var bangumi = await _bangumiRepository.GetAll(x => x.Id == bangumiId).Project<Bangumi>(projection)
                        .FirstOrDefaultAsync();
                    return Mapper.Map<ICollection<AnimeOutlineDto>>(bangumi.Animes);
                }
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
            var filter = Builders<Bangumi>.Filter.Eq("animes._id", animeId);
            var bangumi = await _bangumiRepository.GetAll(filter).FirstOrDefaultAsync();
            Anime anime = null;
            if (bangumi.Animes.Count == 1) anime = bangumi.Animes[0];

            return Mapper.Map<AnimeDto>(anime);
        }


        //
        //        [Route("getlist")]
        //        [HttpGet]
        //        public async Task<ICollection<Anime>> GetList(string bangumiId, int skip, int take)
        //        {
        //            if (skip == 0 && take == 0) return (await _bangumiRepository.GetAsync(bangumiId)).Animes;
        //            var query = Builders<Bangumi>.Projection.Slice(x => x.Animes, skip, take);
        //            var bangumi = await _bangumiRepository
        //                .GetAll(x => x.Id == bangumiId)
        //                .Project<Bangumi>(query).FirstOrDefaultAsync();
        //            return bangumi.Animes;
        //        }
        //
        //        [Route("{bangumiId}/update")]
        //        [HttpPost]
        //        public async Task Update(string bangumiId, [FromBody] Anime input)
        //        {
        //            var filter = Builders<Bangumi>.Filter.Eq("_id", RepositoryHelper.GetInternalId(bangumiId)) &
        //                         Builders<Bangumi>.Filter.Eq("animes._id", RepositoryHelper.GetInternalId(input.Id));
        //            var update = Builders<Bangumi>.Update.Set("animes.$", input);
        //            await _bangumiRepository.UpdateAsync(filter, update);
        //        }
        //
        //        [Route("getdetail")]
        //        [HttpGet]
        //        public async Task<AnimeDetail> GetDetail(string animeId)
        //        {
        //            return await _animeDetailRepository.GetAsync(x => x.AnimeId == animeId);
        //        }
        //
        //        [Route("addresource")]
        //        [HttpPut]
        //        public async Task AddResource([FromBody] AddResourceDto input)
        //        {
        //            foreach (var source in input.Resources) source.Uid = Guid.NewGuid().ToString("N");
        //
        //            var filter = Builders<AnimeDetail>
        //                .Filter.Eq(e => e.AnimeId, input.AnimeId);
        //            var update = input.SourceType == SourceType.Anime
        //                ? Builders<AnimeDetail>.Update.PushEach(x => x.Animes, input.Resources)
        //                : input.SourceType == SourceType.Comic
        //                    ? Builders<AnimeDetail>.Update.PushEach(x => x.Comics, input.Resources)
        //                    : Builders<AnimeDetail>.Update.PushEach(x => x.Novels, input.Resources);
        //
        //            await _animeDetailRepository.UpdateAsync(filter, update);
        //        }
        //
        //        [Route("updateDetail")]
        //        [HttpPost]
        //        public async Task UpdateDetail([FromBody] AnimeDetail input)
        //        {
        //            if (input == null) throw new Exception($"Invalid param {nameof(input)}");
        //            if (_animeDetailRepository.GetAsync(input.Id) == null)
        //                throw new Exception($"The id {input.Id} not exists.");
        //            foreach (var anime in input.Animes)
        //                if (string.IsNullOrEmpty(anime.Uid))
        //                    anime.Uid = Guid.NewGuid().ToString("N");
        //
        //            await _animeDetailRepository.UpdateAsync(input);
        //        }
    }

    public class AddResourceDto
    {
        public string AnimeId { get; set; }

        public SourceType SourceType { get; set; }

        // public List<Resource> Resources { get; set; }
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