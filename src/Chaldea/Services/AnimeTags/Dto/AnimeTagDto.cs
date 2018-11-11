using System.Collections.Generic;

namespace Chaldea.Services.AnimeTags.Dto
{
    public class AnimeTagDto
    {
        public List<string> Types { get; set; }

        public List<string> Tags { get; set; }

        public List<string> States { get; set; }

        public List<int> Levels { get; set; }
    }
}