using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Chaldea.Core.Nodes;
using Chaldea.Node.Configuration;
using Chaldea.Node.Utilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharpCompress.Readers;

namespace Chaldea.Node.Services
{
    public class DirectoryService
    {
        private readonly ILogger<DirectoryService> _logger;
        private readonly IOptions<Appsettings> _options;

        public DirectoryService(
            ILogger<DirectoryService> logger,
            IOptions<Appsettings> options)
        {
            _logger = logger;
            _options = options;
        }

        public ICollection<DirFileInfo> GetDirFiles(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                var rootDirs = new List<DirFileInfo>();
                foreach (var rootDir in _options.Value.RootDirs)
                {
                    var dirInfo = new DirectoryInfo(rootDir);
                    rootDirs.Add(new DirFileInfo
                    {
                        ModifyTime = dirInfo.LastWriteTime, Name = dirInfo.Name, FullName = dirInfo.FullName,
                        Type = DirFileType.Directory
                    });
                }

                return rootDirs;
            }

            var dir = new DirectoryInfo(path);
            var list = dir.GetDirectories().Select(x => new DirFileInfo
                    {ModifyTime = x.LastWriteTime, Name = x.Name, Type = DirFileType.Directory, FullName = x.FullName})
                .OrderBy(x => x.Name).ToList();
            var files = dir.GetFiles().Select(x => new DirFileInfo
            {
                ModifyTime = x.LastWriteTime, Name = x.Name, Size = FileHelper.GetSize(x.Length), FullName = x.FullName,
                Type = DirFileType.File
            }).ToList();
            list.AddRange(files);
            return list;
        }

        public string DeleteDirFiles(ICollection<DirFileInfo> dirFiles)
        {
            var sb = new StringBuilder();
            foreach (var dirFile in dirFiles)
                try
                {
                    if (dirFile.Type == DirFileType.Directory)
                        if (Directory.Exists(dirFile.FullName))
                            Directory.Delete(dirFile.FullName, true);
                        else
                            sb.AppendLine($"Directory {dirFile.Name} not exist.");

                    if (dirFile.Type == DirFileType.File)
                        if (File.Exists(dirFile.FullName))
                            File.Delete(dirFile.FullName);
                        else
                            sb.AppendLine($"File {dirFile.Name} not exist.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                    sb.AppendLine(ex.Message);
                }

            return sb.ToString();
        }

        public string ExtractFiles(ExtractFileInfo extractFileInfo)
        {
            var sb = new StringBuilder();
            if (!Directory.Exists(extractFileInfo.DestDir))
            {
                sb.AppendLine("Dest dir not exist.");
                return sb.ToString();
            }

            foreach (var dirFile in extractFileInfo.Files)
                try
                {
                    if (dirFile.Type == DirFileType.File)
                    {
                        if (!File.Exists(dirFile.FullName))
                            sb.AppendLine($"File {dirFile.Name} not exist, skipped.");
                        else
                            Compress.Extract(dirFile.FullName, extractFileInfo.DestDir,
                                new ReaderOptions {Password = extractFileInfo.Password});
                    }
                    else
                    {
                        sb.AppendLine($"{dirFile.Name} is not file, skipped.");
                    }
                }
                catch (Exception ex)
                {
                    sb.AppendLine(ex.Message);
                }

            return sb.ToString();
        }
    }
}