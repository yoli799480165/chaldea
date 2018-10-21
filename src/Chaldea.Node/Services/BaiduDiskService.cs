using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Chaldea.Core.Nodes;
using Chaldea.Node.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Chaldea.Node.Services
{
    public class BaiduDiskService
    {
        private readonly ILogger<BaiduDiskService> _logger;
        private readonly BaiduDiskSettings _settings;
        private readonly Dictionary<string, Task> _syncTasks = new Dictionary<string, Task>();

        public BaiduDiskService(
            ILogger<BaiduDiskService> logger,
            IOptions<BaiduDiskSettings> options)
        {
            _logger = logger;
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
                    CreateNoWindow = true,
                    StandardOutputEncoding = Encoding.UTF8
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
            const string regex =
                @"\s{2}(\d)+\s{4,}(\w+\.?\w+|-)\s{2,}(\d{4}-\d{1,2}-\d{1,2}\s\d{2}:\d{2}:\d{2})\s{2,}(\w+.?\w+)";
            _logger.LogInformation($"{nameof(GetDirFiles)}: {path}");
            var result = string.IsNullOrEmpty(path) ? Run("ls") : Run($"ls {path}");
            _logger.LogInformation($"Get dirfiles successfully, result: \n {result} \n");
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
                        FullName = $"{path}/{groups[4].Value}"
                    });
            }

            return list;
        }

        public string SyncDir(SyncDirectory syncDirectory)
        {
            var result = string.Empty;
            var task = new Task(obj =>
            {
                var syncParam = (SyncDirectory) obj;
                var parentDir = Path.GetDirectoryName(syncDirectory.Local);
                if (string.IsNullOrEmpty(parentDir))
                {
                    _syncTasks.Remove(syncParam.Local);
                    _logger.LogError("Sync destdir can not be empty.");
                    throw new Exception("Sync destdir can not be empty.");
                }

                var process = GetProcess($"d {syncDirectory.Remote} --saveto {parentDir}");
                process.OutputDataReceived += (sender, e) => { _logger.LogInformation(e.Data); };
                _logger.LogInformation($"Sync dir from {syncParam.Remote} to {syncParam.Local}");
                process.Start();
                process.BeginOutputReadLine();
                process.WaitForExit();
                _syncTasks.Remove(syncParam.Local);
            }, syncDirectory);

            if (_syncTasks.TryAdd(syncDirectory.Local, task))
                task.Start();
            else
                result = "The folder is already in sync.";

            return result;
        }
    }
}