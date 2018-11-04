using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Chaldea.Core.Nodes;
using Chaldea.Core.Repositories;
using Chaldea.Exceptions;
using Chaldea.Services.Nodes.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Chaldea.Services.Nodes
{
    [Route("api/node")]
    public class NodeService : ServiceBase
    {
        private readonly IRepository<string, Anime> _animeRepository;
        private readonly ILogger<NodeService> _logger;
        private readonly IRepository<string, NodeConfig> _nodeConfigRepository;
        private readonly NodeManager _nodeManager;
        private readonly NodeProxy _nodeProxy;
        private readonly IRepository<string, Video> _videoRepository;

        public NodeService(
            ILogger<NodeService> logger,
            NodeProxy nodeProxy,
            NodeManager nodeManager,
            IRepository<string, NodeConfig> nodeConfigRepository,
            IRepository<string, Anime> animeRepository,
            IRepository<string, Video> videoRepository)
        {
            _logger = logger;
            _nodeProxy = nodeProxy;
            _nodeManager = nodeManager;
            _nodeConfigRepository = nodeConfigRepository;
            _animeRepository = animeRepository;
            _videoRepository = videoRepository;
        }

        [Route("getNodes")]
        [HttpGet]
        public ICollection<Node> GetNodes()
        {
            return _nodeManager.GetNodes();
        }

        [Authorize(Roles = nameof(UserRoles.Admin))]
        [Route("{nodeId}/getDirFiles")]
        [HttpPost]
        public async Task<ICollection<DirFileInfo>> GetDirFiles(string nodeId, [FromBody] GetDirFileDto input)
        {
            if (input == null)
                throw new UserFriendlyException($"Invalid parameter {nameof(input)}");

            return await _nodeProxy.GetDirFiles(nodeId, input.Path);
        }

        [Authorize(Roles = nameof(UserRoles.Admin))]
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

        [Authorize(Roles = nameof(UserRoles.Admin))]
        [Route("{nodeId}/extractFiles")]
        [HttpPost]
        public async Task<string> ExtractFiles(string nodeId, [FromBody] ExtractFileDto input)
        {
            if (input == null)
                throw new UserFriendlyException($"Invalid parameter {nameof(input)}");
            var extractFileInfo = Mapper.Map<ExtractFileInfo>(input);

            return await _nodeProxy.ExtractFiles(nodeId, extractFileInfo);
        }

        [Authorize(Roles = nameof(UserRoles.Admin))]
        [Route("{nodeId}/getNetDiskDirFiles")]
        [HttpPost]
        public async Task<ICollection<DirFileInfo>> GetNetDiskDirFiles(string nodeId, [FromBody] GetDirFileDto input)
        {
            if (input == null)
                throw new UserFriendlyException($"Invalid parameter {nameof(input)}");

            return await _nodeProxy.GetNetDiskDirFiles(nodeId, input.Path);
        }

        [Authorize(Roles = nameof(UserRoles.Admin))]
        [Route("{nodeId}/bindSyncDir")]
        [HttpPost]
        public async Task BindSyncDir(string nodeId, [FromBody] SyncDirectory input)
        {
            if (string.IsNullOrEmpty(nodeId))
                throw new UserFriendlyException($"Invalid parameter {nameof(nodeId)}");

            if (input == null)
                throw new UserFriendlyException($"Invalid parameter {nameof(input)}");

            var nodeConfig = await _nodeConfigRepository.GetAsync(x => x.Id == nodeId);
            if (nodeConfig == null)
            {
                nodeConfig = new NodeConfig
                {
                    Id = nodeId,
                    SyncDirectories = new List<SyncDirectory>()
                };
                nodeConfig.SyncDirectories.Add(input);
                await _nodeConfigRepository.AddAsync(nodeConfig);
            }
            else
            {
                var exist = nodeConfig.SyncDirectories.FirstOrDefault(x => x.Local == input.Local);
                if (exist == null)
                {
                    var filter = Builders<NodeConfig>.Filter.Eq("_id", nodeId);
                    var update = Builders<NodeConfig>.Update.Push("syncDirectories", input);
                    await _nodeConfigRepository.UpdateAsync(filter, update);
                }
                else
                {
                    var filter = Builders<NodeConfig>.Filter.Eq("_id", nodeId) &
                                 Builders<NodeConfig>.Filter.Eq("syncDirectories.local", exist.Local);
                    var update = Builders<NodeConfig>.Update.Set("syncDirectories.$", input);
                    await _nodeConfigRepository.UpdateAsync(filter, update);
                }
            }
        }

        [Authorize(Roles = nameof(UserRoles.Admin))]
        [Route("{nodeId}/getSyncDirs")]
        [HttpGet]
        public async Task<ICollection<SyncDirectory>> GetSyncDirs(string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId))
                throw new UserFriendlyException($"Invalid parameter {nameof(nodeId)}");

            var nodeConfig = await _nodeConfigRepository.GetAsync(x => x.Id == nodeId);
            return nodeConfig != null ? nodeConfig.SyncDirectories : new List<SyncDirectory>();
        }

        [Authorize(Roles = nameof(UserRoles.Admin))]
        [Route("{nodeId}/syncDir")]
        [HttpPost]
        public async Task<string> SyncDir(string nodeId, [FromBody] SyncDirectory input)
        {
            if (string.IsNullOrEmpty(nodeId))
                throw new UserFriendlyException($"Invalid parameter {nameof(nodeId)}");

            if (input == null)
                throw new UserFriendlyException($"Invalid parameter {nameof(input)}");

            return await _nodeProxy.SyncDir(nodeId, input);
        }

        [Authorize(Roles = nameof(UserRoles.Admin))]
        [Route("{nodeId}/publishResource")]
        [HttpPost]
        public async Task PublishResource(string nodeId, [FromBody] PublishResourceDto input)
        {
            if (string.IsNullOrEmpty(nodeId))
                throw new UserFriendlyException($"Invalid parameter {nameof(nodeId)}");

            if (input == null)
                throw new UserFriendlyException($"Invalid parameter {nameof(input)}");

            var files = input.PublishFiles.Select(x => x.FullName).ToList();
            var videoInfos = await _nodeProxy.GetVideoInfos(nodeId, files);
            var anime = await _animeRepository.GetAsync(x => x.Id == input.AnimeId);
            if (anime.Videos == null) anime.Videos = new List<Resource>();
            var resources = new List<Resource>();
            var videos = input.PublishFiles.Zip(videoInfos, (fileInfo, videoInfo) =>
            {
                if (fileInfo.FullName != videoInfo.FilePath)
                {
                    _logger.LogError("File info does dot match video info.");
                    throw new Exception("File info does dot match video info.");
                }

                if (anime.Videos.Count > 0)
                {
                    var item = anime.Videos.FirstOrDefault(x => x.Name == fileInfo.DisplayName);
                    if (item != null)
                    {
                        var msg = $"Resources {fileInfo.DisplayName} already exist.";
                        _logger.LogError(msg);
                        throw new Exception(msg);
                    }
                }

                var video = new Video
                {
                    FileName = fileInfo.Name,
                    Length = fileInfo.Length,
                    Size = fileInfo.Size,
                    Cover = "",
                    Title = fileInfo.DisplayName,
                    SubTitle = "",
                    Url = fileInfo.Url,
                    Duration = videoInfo.Duration,
                    FrameWidth = videoInfo.FrameWidth,
                    FrameHeight = videoInfo.FrameHeight,
                    FrameRate = videoInfo.FrameRate,
                    Id = Guid.NewGuid().ToString("N")
                };

                resources.Add(new Resource {Id = video.Id, Name = video.Title});

                return video;
            }).ToList();

            await _videoRepository.AddManyAsync(videos);
            if (anime.Videos.Count <= 0)
            {
                var filter = Builders<Anime>.Filter.Eq("_id", anime.Id);
                var update = Builders<Anime>.Update.Set("videos", resources);
                await _animeRepository.UpdateAsync(filter, update);
            }
            else
            {
                var filter = Builders<Anime>.Filter.Eq("_id", anime.Id);
                var update = Builders<Anime>.Update.PushEach("videos", resources);
                await _animeRepository.UpdateAsync(filter, update);
            }
        }
    }
}