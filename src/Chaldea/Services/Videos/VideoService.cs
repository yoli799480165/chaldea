using System.Threading.Tasks;
using AutoMapper;
using Chaldea.Exceptions;
using Chaldea.Repositories;
using Chaldea.Services.Videos.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Chaldea.Services.Videos
{
    [Route("api/video")]
    public class VideoService : ServiceBase
    {
        private readonly IRepository<string, Video> _videoRepository;

        public VideoService(IRepository<string, Video> videoRepository)
        {
            _videoRepository = videoRepository;
        }

        [Route("{id}/getVideo")]
        [HttpGet]
        public async Task<VideoDto> GetVideo(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new UserFriendlyException($"Invalid parameter {nameof(id)}");

            var video = await _videoRepository.GetAsync(x => x.Id == id);
            return Mapper.Map<VideoDto>(video);
        }
    }
}