using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Chaldea.Core.Nodes;
using Chaldea.Node.Configuration;
using Chaldea.Node.Services;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Chaldea.Node
{
    public class NodeAgent
    {
        private readonly ILogger<NodeAgent> _logger;
        private bool _isConnected;

        public NodeAgent(
            ILogger<NodeAgent> logger,
            IOptions<Appsettings> options,
            DirectoryService directoryService,
            BaiduDiskService baiduDiskService,
            FFmpegService fFmpegService)
        {
            _logger = logger;
            Connection = new HubConnectionBuilder()
                .WithUrl($"{options.Value.NodeServer}/node?nodeId={GetId()}")
                .Build();

            Connection.On<Guid, string>("getDirFiles", async (eventId, path) =>
            {
                var rootDirs = directoryService.GetDirFiles(path);
                await Connection.InvokeAsync("getDirFilesResponse", eventId, rootDirs);
            });

            Connection.On<Guid, ICollection<DirFileInfo>>("deleteDirFiles", async (eventId, dirFiles) =>
            {
                var msg = directoryService.DeleteDirFiles(dirFiles);
                await Connection.InvokeAsync("deleteDirFilesResponse", eventId, msg);
            });

            Connection.On<Guid, ExtractFileInfo>("extractFiles",
                async (eventId, extractFileInfo) =>
                {
                    var msg = directoryService.ExtractFiles(extractFileInfo);
                    await Connection.InvokeAsync("extractFilesResponse", eventId, msg);
                });

            Connection.On<Guid, string>("getNetDiskDirFiles", async (eventId, path) =>
            {
                var rootDirs = baiduDiskService.GetDirFiles(path);
                await Connection.InvokeAsync("getNetDiskDirFilesResponse", eventId, rootDirs);
            });

            Connection.On<Guid, SyncDirectory>("syncDir", async (eventId, syncDir) =>
            {
                var msg = baiduDiskService.SyncDir(syncDir);
                await Connection.InvokeAsync("syncDirResponse", eventId, msg);
            });

            Connection.On<Guid, ICollection<string>>("getVideoInfos", async (eventId, files) =>
            {
                var videoInfos = fFmpegService.GetVideoInfos(files);
                await Connection.InvokeAsync("getVideoInfosResponse", eventId, videoInfos);
            });

            Connection.Closed += ex =>
            {
                _isConnected = false;
                _logger.LogInformation($"The connection is closed, {ex}");
                return Task.CompletedTask;
            };
        }

        public HubConnection Connection { get; set; }

        public void Connect()
        {
            Task.Run(async () =>
            {
                for (;;)
                    if (_isConnected)
                        await Task.Delay(1000);
                    else
                        try
                        {
                            await Connection.StartAsync();
                            _isConnected = true;
                            _logger.LogInformation("Connect to server successly.");
                        }
                        catch (Exception)
                        {
                            _isConnected = false;
                            await Task.Delay(1000);
                        }
            });
        }

        private string GetId()
        {
            var nodeId = Guid.NewGuid().ToString("N");
            if (File.Exists("id.txt"))
                nodeId = File.ReadAllText("id.txt");
            else
                File.WriteAllText("id.txt", nodeId);

            return nodeId;
        }
    }
}