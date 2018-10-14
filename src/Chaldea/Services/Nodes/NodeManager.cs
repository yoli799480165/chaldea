using System.Collections.Concurrent;
using System.Collections.Generic;
using Chaldea.Repositories;

namespace Chaldea.Services.Nodes
{
    public class NodeManager
    {
        private readonly ConcurrentDictionary<string, Node> _nodes = new ConcurrentDictionary<string, Node>();

        public void Register(Node node)
        {
            _nodes.AddOrUpdate(node.Id, node, (key, value) => node);
        }

        public void UnRegister(string id)
        {
            if (_nodes.TryGetValue(id, out var item)) item.State = NodeState.Offline;
        }

        public string GetConnectionId(string id)
        {
            if (_nodes.TryGetValue(id, out var item))
            {
                return item.ConnectionId;
            }

            return null;
        }

        public ICollection<Node> GetNodes()
        {
            return _nodes.Values;
        }
    }
}