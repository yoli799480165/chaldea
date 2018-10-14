using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
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

        public async Task<List<string>> GetRootDirs(string nodeId)
        {
            (Guid eventId, Subject<object> subject) result = _eventManager.Create();
            var connectionId = _nodeManager.GetConnectionId(nodeId);
            if (string.IsNullOrEmpty(connectionId)) throw new Exception("ConnectionId is null, node was not found.");
            await _hubContext.Clients.Client(connectionId).SendAsync("getRootDirs", result.eventId);
            var data = await result.subject.GetAwaiter();
            return data as List<string>;
        }

        public async Task<List<string>> GetDirFiles(string nodeId, string path)
        {
            (Guid eventId, Subject<object> subject) result = _eventManager.Create();
            var connectionId = _nodeManager.GetConnectionId(nodeId);
            if (string.IsNullOrEmpty(connectionId)) throw new Exception("ConnectionId is null, node was not found.");
            await _hubContext.Clients.Client(connectionId).SendAsync("getDirFiles", result.eventId, path);
            var data = await result.subject.GetAwaiter();
            return data as List<string>;
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