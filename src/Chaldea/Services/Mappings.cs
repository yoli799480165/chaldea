using System.Collections.Generic;
using AutoMapper;
using Chaldea.Repositories;
using Chaldea.Services.Animes.Dto;
using Chaldea.Services.AnimeTags.Dto;
using Chaldea.Services.Bangumis.Dto;

namespace Chaldea.Services
{
    public class Mappings
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<BangumiEditDto, Bangumi>();
                cfg.CreateMap<BangumiAnimesDto, Bangumi>();
                cfg.CreateMap<AnimeOutlineDto, Anime>();
                cfg.CreateMap<AnimeTagDto, AnimeTag>();
                cfg.CreateMap<ICollection<BangumiDto>, ICollection<Bangumi>>();
                cfg.CreateMap<ICollection<AnimeOutlineDto>, ICollection<Anime>>();
            });
        }
    }
}