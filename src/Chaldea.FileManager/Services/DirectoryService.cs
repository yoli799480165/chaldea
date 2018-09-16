using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Chaldea.FileManager.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NReco.VideoInfo;

namespace Chaldea.FileManager.Services
{
    [Route("api/chaldea/directory")]
    public class DirectoryService : ControllerBase
    {
        private readonly DirectoryManager _directoryManager;

        public DirectoryService(DirectoryManager directoryManager)
        {
            _directoryManager = directoryManager;
        }

        [Route("getFiles")]
        [HttpPost]
        public ICollection<FileDto> GetFiles([FromBody] GetFilesDto input)
        {
            var files = _directoryManager.GetFiles(input.Path).OrderBy(x => x.Name);
            return files.Select(x => new FileDto
            {
                Name = x.Name,
                FullName = x.FullName,
                DirectoryName = x.DirectoryName,
                Size = Filesize(x.Length),
                Media = MediaHelper.GetMediaInfo(x.FullName)
            }).ToList();
        }

        [Route("renameFiles")]
        [HttpPost]
        public IActionResult RenameFiles([FromBody] RenameFilesDto input)
        {
            var files = input.Files.Select(x => new FileInfo(x.FullName)).ToArray();
            _directoryManager.RenameFiles(input.NameFormat, files);
            return Ok();
        }

        [Route("renameDirectory")]
        [HttpPost]
        public IActionResult RenameDirectory([FromBody] RenameDirectoryDto input)
        {
            if (!Directory.Exists(input.SourcePath)) throw new Exception("Source dir not exists.");
            if (Directory.Exists(input.DestPath)) throw new Exception("Dest dir exists.");
            Directory.Move(input.SourcePath, input.DestPath);
            return Ok();
        }

        [Route("searchFiles")]
        [HttpPost]
        public async Task<ICollection<FileDto>> SearchFiles([FromBody] SearchFilesDto input)
        {
            var files = await _directoryManager.SearchFiles(input.Path, input.SearchPattern,
                SearchOption.AllDirectories);
            return files.Select(x => new FileDto
            {
                Name = x.Name,
                FullName = x.FullName,
                DirectoryName = x.DirectoryName,
                Size = Filesize(x.Length)
            }).ToList();
        }

//        [Route("getPath")]
//        [HttpGet]
//        public DirPathDto GetFilePath(GetFilePathDto input)
//        {
//            var path = _directoryManager.GetPath(input.BangumiName, input.AnimeName);
//            return new DirPathDto {Path = path};
//        }

        private string Filesize(double size)
        {
            var units = new[] {"B", "KB", "MB", "GB", "TB", "PB"};
            var mod = 1024.0;
            var i = 0;
            while (size >= mod)
            {
                size /= mod;
                i++;
            }

            return Math.Round(size) + units[i];
        }
    }

//    public class DirPathDto
//    {
//        public string Path { get; set; }
//    }
//
//    public class GetFilePathDto
//    {
//        public string BangumiName { get; set; }
//
//        public string AnimeName { get; set; }
//    }

    public class RenameDirectoryDto
    {
        public string SourcePath { get; set; }

        public string DestPath { get; set; }
    }

    public class RenameFilesDto
    {
        public string NameFormat { get; set; }

        public IList<FileDto> Files { get; set; }
    }

    public class FileDto
    {
        public string Name { get; set; }

        public string FullName { get; set; }

        public string DirectoryName { get; set; }

        public string Size { get; set; }

        public MediaInfo Media { get; set; }
    }

    public class MediaInfo
    {
        public int Duration { get; set; }
        public int FrameWidth { get; set; }
        public int FrameHeight { get; set; }
        public int FrameRate { get; set; }
    }

    public class GetFilesDto
    {
        public string Path { get; set; }
    }

    public class SearchFilesDto
    {
        public string Path { get; set; }

        public string SearchPattern { get; set; }
    }

    public class DirectoryManager
    {
        private readonly ConcurrentDictionary<string, string>
            _animeCache = new ConcurrentDictionary<string, string>();

        private readonly ConcurrentDictionary<string, string>
            _bangumiCache = new ConcurrentDictionary<string, string>();

        public DirectoryManager(IOptions<Appsettings> options)
        {
            // Task.Run(() => { InitDirCache(options.Value.ScanPath); });
        }

        public ICollection<FileInfo> GetFiles(string path)
        {
            var dir = new DirectoryInfo(path);
            return dir.GetFiles();
        }

        public void RenameFiles(string nameFormat, FileInfo[] files)
        {
            var totalWidth = files.Length.ToString().Length;
            for (var i = 0; i < files.Length; i++)
            {
                var file = files[i];
                var index = (i + 1).ToString();
                var newName = string.Format(nameFormat + file.Extension, index.PadLeft(totalWidth, '0'));
                var destPath = Path.Combine(file.DirectoryName, newName);
                if (!File.Exists(destPath)) file.MoveTo(destPath);
            }
        }

        public async Task<ICollection<FileInfo>> SearchFiles(string path, string searchPattern, SearchOption option)
        {
            var result = await Task.Factory.StartNew(() =>
            {
                var dir = new DirectoryInfo(path);
                return dir.GetFileSystemInfos(searchPattern, option);
            });

            return result.Select(x => new FileInfo(x.FullName)).ToList();
        }

        public string GetPath(string bangumiName, string animeName)
        {
            // todo: change to search model
            if (_bangumiCache.TryGetValue(bangumiName, out var bangumiDir))
            {
                if (_animeCache.TryGetValue(animeName, out var anime)) return anime;

                return bangumiDir;
            }

            return string.Empty;
        }

        public void InitDirCache(List<string> paths)
        {
            _bangumiCache.Clear();
            foreach (var path in paths)
            {
                var bangumis = Directory.GetDirectories(path);
                foreach (var bangumi in bangumis)
                {
                    var bangumiName = Path.GetDirectoryName(bangumi);
                    _bangumiCache.TryAdd(bangumiName, bangumi);

                    var animes = Directory.GetDirectories(bangumi);

                    foreach (var anime in animes)
                    {
                        var animeName = Path.GetDirectoryName(bangumi);
                        _animeCache.TryAdd(animeName, anime);
                    }
                }
            }
        }

        // private void SearchFiles(DirectoryInfo root, string searchPattern, List<FileInfo> fileList)
        // {
        //     var dirs = root.GetDirectories();
        //     if (dirs.Length > 0)
        //     {
        //         foreach (var dir in dirs)
        //         {
        //             SearchFiles(dir, searchPattern, fileList);
        //         }
        //     }

        //     var files = root.GetFiles(searchPattern);
        //     if (files.Length > 0)
        //     {
        //         fileList.AddRange(files);
        //     }
        // }
    }

    public static class MediaHelper
    {
        private static readonly FFProbe FfProbe = new NReco.VideoInfo.FFProbe();

        public static MediaInfo GetMediaInfo(string path)
        {
            var media = new MediaInfo();
            var videoInfo = FfProbe.GetMediaInfo(path);
            media.Duration = Convert.ToInt32(videoInfo.Duration.TotalSeconds);
            if (videoInfo.Streams.Length > 0)
            {
                var frame = videoInfo.Streams[0];
                media.FrameHeight = frame.Height;
                media.FrameWidth = frame.Width;
                media.FrameRate = Convert.ToInt32(frame.FrameRate);
            }

            return media;
        }
    }
}