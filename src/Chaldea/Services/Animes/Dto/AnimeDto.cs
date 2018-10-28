using System.Collections.Generic;
using Chaldea.Core.Repositories;

namespace Chaldea.Services.Animes.Dto
{
    public class AnimeDto
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Cover { get; set; }

        public long PlayCounts { get; set; }

        public long SubCounts { get; set; }

        public string Desc { get; set; }

        public string Auth { get; set; }

        public string Publisher { get; set; }

        public string Director { get; set; }

        public string State { get; set; }

        public string Type { get; set; }

        public int Level { get; set; }

        public List<string> Tags { get; set; }

        public List<Resource> Videos { get; set; }

        public List<Resource> Comics { get; set; }

        public List<Resource> Novels { get; set; }

        public List<string> Comments { get; set; }
    }
}