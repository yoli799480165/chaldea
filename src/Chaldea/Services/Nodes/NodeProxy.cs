using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Chaldea.Core.Nodes;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Chaldea.Services.Nodes
{
    public class NodeProxy
    {
        private readonly EventManager _eventManager;
        private readonly IHubContext<NodeHub> _hubContext;
        private readonly NodeManager _nodeManager;

        public NodeProxy(
            IHubContext<NodeHub> hubContext,
            EventManager eventManager,
            NodeManager nodeManager)
        {
            _hubContext = hubContext;
            _eventManager = eventManager;
            _nodeManager = nodeManager;
        }

        public async Task<List<DirFileInfo>> GetDirFiles(string nodeId, string path)
        {
            (Guid eventId, Subject<object> subject) result = _eventManager.Create();
            var connectionId = _nodeManager.GetConnectionId(nodeId);
            if (string.IsNullOrEmpty(connectionId)) throw new Exception("ConnectionId is null, node was not found.");
            await _hubContext.Clients.Client(connectionId).SendAsync("getDirFiles", result.eventId, path);
            var data = await result.subject.GetAwaiter();
            return data as List<DirFileInfo>;
        }

        public async Task<string> DeleteDirFiles(string nodeId, ICollection<DirFileInfo> dirFiles)
        {
            (Guid eventId, Subject<object> subject) result = _eventManager.Create();
            var connectionId = _nodeManager.GetConnectionId(nodeId);
            if (string.IsNullOrEmpty(connectionId)) throw new Exception("ConnectionId is null, node was not found.");
            await _hubContext.Clients.Client(connectionId).SendAsync("deleteDirFiles", result.eventId, dirFiles);
            var data = await result.subject.GetAwaiter();
            return data as string;
        }

        public async Task<string> ExtractFiles(string nodeId, ExtractFileInfo extractFileInfo)
        {
            (Guid eventId, Subject<object> subject) result = _eventManager.Create();
            var connectionId = _nodeManager.GetConnectionId(nodeId);
            if (string.IsNullOrEmpty(connectionId)) throw new Exception("ConnectionId is null, node was not found.");
            await _hubContext.Clients.Client(connectionId).SendAsync("extractFiles", result.eventId, extractFileInfo);
            var data = await result.subject.GetAwaiter();
            return data as string;
        }
    }

    public class EventManager
    {
        private readonly Dictionary<Guid, Subject<object>> _events = new Dictionary<Guid, Subject<object>>();
        private readonly ILogger<EventManager> _logger;

        public EventManager(
            ILogger<EventManager> logger)
        {
            _logger = logger;
        }

        public (Guid, Subject<object>) Create()
        {
            var eventId = Guid.NewGuid();
            var subject = new Subject<object>();
            _events.Add(eventId, subject);
            return (eventId, subject);
        }

        public void Emit(Guid eventId, object data)
        {
            if (_events.TryGetValue(eventId, out var item))
            {
                _logger.LogInformation($"Event: {eventId} was emited.");
                item.OnNext(data);
                item.OnCompleted();
                _events.Remove(eventId);
            }
        }
    }
}