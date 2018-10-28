using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chaldea.Core.Nodes;
using Chaldea.Core.Repositories;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Chaldea.Services.Nodes
{
    public class NodeHub : Hub
    {
        private readonly EventManager _eventManager;
        private readonly ILogger<NodeHub> _logger;
        private readonly NodeManager _nodeManager;

        public NodeHub(
            ILogger<NodeHub> logger,
            NodeManager nodeManager,
            EventManager eventManager)
        {
            _logger = logger;
            _nodeManager = nodeManager;
            _eventManager = eventManager;
        }

        public override Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var address = httpContext.Connection.RemoteIpAddress.IsIPv4MappedToIPv6
                ? httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString()
                : httpContext.Connection.RemoteIpAddress.ToString();
            var node = new Node
            {
                Id = httpContext.Request.Query["nodeId"],
                OsType = httpContext.Request.Query["osType"],
                Ip = address,
                Name = address,
                ConnectionId = Context.ConnectionId
            };
            _logger.LogInformation($"Node {node.Id} conected, ip: {node.Ip}");
            _nodeManager.Register(node);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var httpContext = Context.GetHttpContext();
            var id = httpContext.Request.Query["nodeId"];
            _nodeManager.UnRegister(id);
            _logger.LogInformation($"Node {id} disconnected.");
            return base.OnDisconnectedAsync(exception);
        }

        public Task GetDirFilesResponse(Guid eventId, List<DirFileInfo> data)
        {
            var httpContext = Context.GetHttpContext();
            var id = httpContext.Request.Query["nodeId"];
            _logger.LogInformation($"Node {id} call method: {nameof(GetDirFilesResponse)}");
            _eventManager.Emit(eventId, data);
            return Task.CompletedTask;
        }

        public Task DeleteDirFilesResponse(Guid eventId, string msg)
        {
            var httpContext = Context.GetHttpContext();
            var id = httpContext.Request.Query["nodeId"];
            _logger.LogInformation($"Node {id} call method: {nameof(DeleteDirFilesResponse)}");
            _eventManager.Emit(eventId, msg);
            return Task.CompletedTask;
        }

        public Task ExtractFilesResponse(Guid eventId, string msg)
        {
            var httpContext = Context.GetHttpContext();
            var id = httpContext.Request.Query["nodeId"];
            _logger.LogInformation($"Node {id} call method: {nameof(ExtractFilesResponse)}");
            _eventManager.Emit(eventId, msg);
            return Task.CompletedTask;
        }


        public Task GetNetDiskDirFilesResponse(Guid eventId, List<DirFileInfo> data)
        {
            var httpContext = Context.GetHttpContext();
            var id = httpContext.Request.Query["nodeId"];
            _logger.LogInformation($"Node {id} call method: {nameof(GetNetDiskDirFilesResponse)}");
            _eventManager.Emit(eventId, data);
            return Task.CompletedTask;
        }

        public Task SyncDirResponse(Guid eventId, string msg)
        {
            var httpContext = Context.GetHttpContext();
            var id = httpContext.Request.Query["nodeId"];
            _logger.LogInformation($"Node {id} call method: {nameof(SyncDirResponse)}");
            _eventManager.Emit(eventId, msg);
            return Task.CompletedTask;
        }

        public Task GetVideoInfosResponse(Guid eventId, ICollection<VideoInfo> data)
        {
            var httpContext = Context.GetHttpContext();
            var id = httpContext.Request.Query["nodeId"];
            _logger.LogInformation($"Node {id} call method: {nameof(GetVideoInfosResponse)}");
            _eventManager.Emit(eventId, data);
            return Task.CompletedTask;
        }
    }
}