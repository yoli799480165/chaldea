using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Chaldea.Core.Repositories;
using Chaldea.Exceptions;
using Chaldea.Services.Comments.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Chaldea.Services.Comments
{
    [Route("api/comment")]
    public class CommentService : ServiceBase
    {
        private readonly IRepository<string, Comment> _commentRepository;
        private readonly ILogger<CommentService> _logger;

        public CommentService(
            ILogger<CommentService> logger,
            IRepository<string, Comment> commentRepository)
        {
            _logger = logger;
            _commentRepository = commentRepository;
        }

        [Authorize]
        [Route("addComment")]
        [HttpPut]
        public async Task AddComment([FromBody] CommentAddDto input)
        {
            if (input == null) throw new UserFriendlyException($"Invalid parameter {nameof(input)}");

            try
            {
                var comment = Mapper.Map<Comment>(input);
                comment.CreationTime = DateTime.UtcNow;
                comment.Id = Guid.NewGuid().ToString("N");
                comment.UserId = UserId;
                comment.Likes = new List<string>();
                comment.Unlikes = new List<string>();
                comment.Replies = new List<Comment>();

                await _commentRepository.AddAsync(comment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new UserFriendlyException("Add comment failed.");
            }
        }

//        [Authorize]
//        [Route("reply")]
//        [HttpPut]
//        public async Task Reply([FromBody] CommentAddDto input)
//        {
//
//        }

        [Route("getComments")]
        [HttpGet]
        public async Task<ICollection<CommentDto>> GetComments(string targetId, int skip, int take)
        {
            try
            {
                var stages = new List<string>
                {
                    "{$match:{'targetId':'" + targetId + "'}}",
                    "{$sort:{'creationTime':-1}}",
                    "{$lookup:{from:'users',localField:'userId',foreignField:'_id',as:'user'}}",
                    "{$unwind:'$user'}",
                    "{$project:{'_id':1,'userId':1,'userName':'$user.name','avatar':'$user.avatar','content':1,'index':1,'likes':1,'unlikes':1,'replies':1,'creationTime':1}}"
                };

                if (skip >= 0 && take > 0)
                    stages.InsertRange(1, new[]
                    {
                        "{$skip:" + skip + "}",
                        "{$limit:" + take + "}"
                    });

                var pipeline = PipelineDefinition<Comment, CommentDto>.Create(stages);
                var favorites = await _commentRepository.Aggregate(pipeline).ToListAsync();
                return favorites;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new UserFriendlyException("get comments failed.");
            }
        }
    }
}