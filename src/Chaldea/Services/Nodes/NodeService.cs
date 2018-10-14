using System.Collections.Generic;
using System.Threading.Tasks;
using Chaldea.Repositories;
using Chaldea.Services.Nodes.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Chaldea.Services.Nodes
{
    [Route("api/node")]
    public class NodeService : ServiceBase
    {
        private readonly ILogger<NodeService> _logger;
        private readonly NodeManager _nodeManager;
        private readonly NodeProxy _nodeProxy;

        public NodeService(
            ILogger<NodeService> logger,
            NodeProxy nodeProxy,
            NodeManager nodeManager)
        {
            _logger = logger;
            _nodeProxy = nodeProxy;
            _nodeManager = nodeManager;
        }

        [Route("getNodes")]
        [HttpGet]
        public ICollection<Node> GetNodes()
        {
            return _nodeManager.GetNodes();
        }

        [Route("{nodeId}/getRootDirs")]
        [HttpGet]
        public async Task<ICollection<string>> GetRootDirs(string nodeId)
        {
            return await _nodeProxy.GetRootDirs(nodeId);
        }

        [Route("{nodeId}/getDirFiles")]
        [HttpPost]
        public async Task<ICollection<string>> GetDirFiles(string nodeId, [FromBody] GetDirFileDto input)
        {
            return await _nodeProxy.GetDirFiles(nodeId, input.Path);
        }
    }
}