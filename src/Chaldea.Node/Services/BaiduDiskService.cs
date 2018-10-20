using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using Chaldea.Core.Nodes;
using Chaldea.Node.Configuration;
using Microsoft.Extensions.Options;

namespace Chaldea.Node.Services
{
    public class BaiduDiskService
    {
        private readonly BaiduDiskSettings _settings;

        public BaiduDiskService(IOptions<BaiduDiskSettings> options)
        {
            _settings = options.Value;
        }

        private Process GetProcess(string cmd)
        {
            return new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = _settings.ToolPath,
                    Arguments = cmd,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
        }

        public string Run(string cmd)
        {
            var process = GetProcess(cmd);
            process.Start();
            var result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }

        public ICollection<DirFileInfo> GetDirFiles(string path)
        {
            var regex =
                @"\s{2}(\d)+\s{4,}(\w+\.?\w+|-)\s{2,}(\d{4}-\d{1,2}-\d{1,2}\s\d{2}:\d{2}:\d{2})\s{2,}(\w+.?\w+)";

            var result = string.IsNullOrEmpty(path) ? Run("ls") : Run($"ls {path}");
            var matches = Regex.Matches(result, regex);
            var list = new List<DirFileInfo>();
            foreach (Match match in matches)
            {
                var groups = match.Groups;
                if (groups.Count > 1)
                    list.Add(new DirFileInfo
                    {
                        Name = groups[4].Value,
                        Size = groups[2].Value,
                        ModifyTime = Convert.ToDateTime(groups[3].Value),
                        Type = groups[2].Value == "-" ? DirFileType.Directory : DirFileType.File,
                        FullName = Path.Combine(string.IsNullOrEmpty(path) ? "/" : path, groups[4].Value)
                    });
            }

            return list;
        }
    }
}