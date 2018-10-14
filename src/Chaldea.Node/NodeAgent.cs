using System;
using System.IO;
using System.Threading.Tasks;
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

            Connection.On<Guid>("getRootDirs", async eventId =>
            {
                var rootDirs = directoryService.GetRootDirs();
                await Connection.InvokeAsync("getRootDirsResponse", eventId, rootDirs);
            });

            Connection.On<Guid, string>("getDirFiles", async (eventId, path) =>
            {
                var rootDirs = directoryService.GetDirFiles(path);
                await Connection.InvokeAsync("getDirFilesResponse", eventId, rootDirs);
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