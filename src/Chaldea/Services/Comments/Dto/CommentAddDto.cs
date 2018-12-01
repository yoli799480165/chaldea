namespace Chaldea.Services.Comments.Dto
{
    public class CommentAddDto
    {
        public string TargetId { get; set; }

        public string ParentId { get; set; }

        public string Content { get; set; }
    }
}