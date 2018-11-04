using System;
using Chaldea.Services.Animes.Dto;

namespace Chaldea.Services.Timetables.Dto
{
    public class TimetableDto
    {
        public AnimeOutlineDto Anime { get; set; }

        public string SourceUrl { get; set; }

        public string SourcePwd { get; set; }

        public TimeSpan UpdateTime { get; set; }

        public DayOfWeek UpdateWeek { get; set; }

        public string WeekName { get; set; }

        public string Id { get; set; }
    }
}