namespace Chaldea.Services.Videos.Dto
{
    public class VideoDto
    {
        public string Size { get; set; }

        public string Cover { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public int Duration { get; set; }

        public int FrameWidth { get; set; }

        public int FrameHeight { get; set; }

        public int FrameRate { get; set; }

        public int CurrentTime { get; set; }

        public string Id { get; set; }
    }
}