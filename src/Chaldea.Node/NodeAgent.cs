using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Chaldea.Core.Nodes;
using Chaldea.Node.Configuration;
using Chaldea.Node.Services;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;

namespace Chaldea.Node
{
    public class NodeAgent
    {
        private bool _isConnected;

        public NodeAgent(
            IOptions<Appsettings> options,
            DirectoryService directoryService)
        {
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