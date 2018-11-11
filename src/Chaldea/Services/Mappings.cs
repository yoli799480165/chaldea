using System.Collections.Generic;
using AutoMapper;
using Chaldea.Core.Repositories;
using Chaldea.Services.Animes.Dto;
using Chaldea.Services.AnimeTags.Dto;
using Chaldea.Services.Bangumis.Dto;
using Chaldea.Services.Histories.Dto;
using Chaldea.Services.Users.Dto;
using Chaldea.Services.Videos.Dto;

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
                cfg.CreateMap<AnimeDto, Anime>();
                cfg.CreateMap<AnimeOutlineDto, Anime>();
                cfg.CreateMap<AnimeTagDto, AnimeTag>();
                cfg.CreateMap<Video, VideoDto>().ForMember(x => x.CurrentTime, opt => opt.Ignore());
                cfg.CreateMap<UserDto, User>();
                cfg.CreateMap<ICollection<UserDto>, ICollection<User>>();
                cfg.CreateMap<ICollection<BangumiDto>, ICollection<Bangumi>>();
                cfg.CreateMap<ICollection<AnimeOutlineDto>, ICollection<Anime>>();
                cfg.CreateMap<ICollection<HistoryDto>, ICollection<History>>();
            });
        }
    }
}