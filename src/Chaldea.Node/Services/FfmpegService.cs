using System;
using System.Collections.Generic;
using Chaldea.Core.Nodes;
using Chaldea.Node.Configuration;
using Microsoft.Extensions.Options;
using NReco.VideoInfo;

namespace Chaldea.Node.Services
{
    public class FFmpegService
    {
        private readonly FFProbe _ffProbe = new FFProbe();

        public FFmpegService(IOptions<FFmpegSettings> options)
        {
            _ffProbe.ToolPath = options.Value.ToolPath;
            _ffProbe.FFProbeExeName = options.Value.FFProbe;
        }

        public ICollection<VideoInfo> GetVideoInfos(ICollection<string> files)
        {
            var list = new List<VideoInfo>();
            foreach (var file in files)
            {
                var mediaInfo = _ffProbe.GetMediaInfo(file);
                var videoInfo = new VideoInfo
                {
                    FilePath = file,
                    Duration = Convert.ToInt32(mediaInfo.Duration.TotalSeconds)
                };
                if (mediaInfo.Streams.Length > 0)
                {
                    var frame = mediaInfo.Streams[0];
                    videoInfo.FrameHeight = frame.Height;
                    videoInfo.FrameWidth = frame.Width;
                    videoInfo.FrameRate = Convert.ToInt32(frame.FrameRate);
                }

                list.Add(videoInfo);
            }

            return list;
        }
    }
}