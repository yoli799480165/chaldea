﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chaldea.Repositories;
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
            var node = new Node
            {
                Id = httpContext.Request.Query["nodeId"],
                OsType = httpContext.Request.Query["osType"],
                Ip = httpContext.Connection.RemoteIpAddress.ToString(),
                Name = httpContext.Connection.RemoteIpAddress.ToString(),
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

        public Task GetRootDirsResponse(Guid eventId, List<string> data)
        {
            var httpContext = Context.GetHttpContext();
            var id = httpContext.Request.Query["nodeId"];
            _logger.LogInformation($"Node {id} call method: {nameof(GetRootDirsResponse)}");
            _eventManager.Emit(eventId, data);
            return Task.CompletedTask;
        }

        public Task GetDirFilesResponse(Guid eventId, List<string> data)
        {
            var httpContext = Context.GetHttpContext();
            var id = httpContext.Request.Query["nodeId"];
            _logger.LogInformation($"Node {id} call method: {nameof(GetDirFilesResponse)}");
            _eventManager.Emit(eventId, data);
            return Task.CompletedTask;
        }
    }
}