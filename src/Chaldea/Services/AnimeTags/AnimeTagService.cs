using System;
using System.Threading.Tasks;
using AutoMapper;
using Chaldea.Core.Repositories;
using Chaldea.Exceptions;
using Chaldea.Services.AnimeTags.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Chaldea.Services.AnimeTags
{
    [Route("api/animeTag")]
    public class AnimeTagService : ServiceBase
    {
        private readonly IRepository<string, AnimeTag> _animeTagRepository;
        private readonly ILogger<AnimeTagService> _logger;

        public AnimeTagService(
            ILogger<AnimeTagService> logger,
            IRepository<string, AnimeTag> animeTagRepository)
        {
            _logger = logger;
            _animeTagRepository = animeTagRepository;
        }

        [Route("getTags")]
        [HttpGet]
        public async Task<AnimeTagDto> GetTags()
        {
            try
            {
                var animeTagList = await _animeTagRepository.GetAllListAsync();
                if (animeTagList != null && animeTagList.Count > 0) return Mapper.Map<AnimeTagDto>(animeTagList[0]);

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new UserFriendlyException("Get anime tags failed.");
            }
        }
    }
}