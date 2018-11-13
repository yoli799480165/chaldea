using System;

namespace Chaldea.Services.Histories.Dto
{
    public class HistoryDetailDto
    {
        public int Duration { get; set; }

        public int CurrentTime { get; set; }

        public string AnimeId { get; set; }

        public string AnimeTitle { get; set; }

        public string SourceTitle { get; set; }

        public string Screenshot { get; set; }

        public DateTime LastModificationTime { get; set; }

        public string Id { get; set; }
    }
}