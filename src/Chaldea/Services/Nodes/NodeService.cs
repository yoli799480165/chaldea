using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Chaldea.Core.Nodes;
using Chaldea.Exceptions;
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

        [Route("{nodeId}/getDirFiles")]
        [HttpPost]
        public async Task<ICollection<DirFileInfo>> GetDirFiles(string nodeId, [FromBody] GetDirFileDto input)
        {
            if (input == null)
                throw new UserFriendlyException($"Invalid parameter {nameof(input)}");

            return await _nodeProxy.GetDirFiles(nodeId, input.Path);
        }

        [Route("{nodeId}/deleteDirFiles")]
        [HttpDelete]
        public async Task<string> DeleteDirFiles(string nodeId, [FromBody] ICollection<DirFileInfo> input)
        {
            if (input == null)
                throw new UserFriendlyException($"Invalid parameter {nameof(input)}");

            if (input.Count <= 0)
                throw new UserFriendlyException($"Invalid parameter length {input.Count}");

            return await _nodeProxy.DeleteDirFiles(nodeId, input);
        }

        [Route("{nodeId}/extractFiles")]
        [HttpDelete]
        public async Task<string> ExtractFiles(string nodeId, [FromBody] ExtractFileDto input)
        {
            if (input == null)
                throw new UserFriendlyException($"Invalid parameter {nameof(input)}");
            var extractFileInfo = Mapper.Map<ExtractFileInfo>(input);

            return await _nodeProxy.ExtractFiles(nodeId, extractFileInfo);
        }
    }
}