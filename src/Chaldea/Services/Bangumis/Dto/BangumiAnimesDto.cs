using System.Collections.Generic;
using Chaldea.Services.Animes.Dto;

namespace Chaldea.Services.Bangumis.Dto
{
    public class BangumiAnimesDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public List<AnimeOutlineDto> Animes { get; set; }
    }
}