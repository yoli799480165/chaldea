using System.Threading.Tasks;
using AutoMapper;
using Chaldea.Core.Repositories;
using Chaldea.Exceptions;
using Chaldea.Services.Videos.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Chaldea.Services.Videos
{
    [Route("api/video")]
    public class VideoService : ServiceBase
    {
        private readonly IRepository<string, Anime> _animeRepository;
        private readonly IRepository<string, History> _historyRepository;
        private readonly IRepository<string, Video> _videoRepository;

        public VideoService(
            IRepository<string, Video> videoRepository,
            IRepository<string, Anime> animeRepository,
            IRepository<string, History> historyRepository)
        {
            _videoRepository = videoRepository;
            _animeRepository = animeRepository;
            _historyRepository = historyRepository;
        }

        [AllowAnonymous]
        [Route("{id}/getVideoForPlayer")]
        [HttpGet]
        public async Task<VideoDto> GetVideoForPlayer(string animeId, string id)
        {
            if (string.IsNullOrEmpty(id)) throw new UserFriendlyException($"Invalid parameter {nameof(id)}");
            // 更新播放量
            var filter = Builders<Anime>.Filter.Eq("_id", animeId);
            var update = Builders<Anime>.Update.Inc(x => x.PlayCounts, 1);
            await _animeRepository.UpdateAsync(filter, update);

            // 获取视频
            var video = await _videoRepository.GetAsync(x => x.Id == id);
            var dto = Mapper.Map<VideoDto>(video);

            // 获取播放历史
            if (IsLogin)
            {
                var history = await _historyRepository.GetAsync(x => x.UserId == UserId && x.ResourceId == id);
                if (history != null) dto.CurrentTime = history.CurrentTime;
            }

            return dto;
        }
    }
}