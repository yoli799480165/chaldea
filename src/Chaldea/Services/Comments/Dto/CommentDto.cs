using System;
using System.Collections.Generic;

namespace Chaldea.Services.Comments.Dto
{
    public class CommentDto
    {
        public DateTime CreationTime { get; set; }

        public List<CommentDto> Replies { get; set; }

        public List<string> Likes { get; set; }

        public List<string> Unlikes { get; set; }

        public int Index { get; set; }

        public string Content { get; set; }

        public string Avatar { get; set; }

        public string UserName { get; set; }

        public string UserId { get; set; }

        public string Id { get; set; }
    }
}