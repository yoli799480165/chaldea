using System.Collections.Generic;
using Chaldea.Core.Nodes;

namespace Chaldea.Services.Nodes.Dto
{
    public class PublishDirFileInfo : DirFileInfo
    {
        public string DisplayName { get; set; }

        public string Url { get; set; }
    }

    public class PublishResourceDto
    {
        public string AnimeId { get; set; }

        public bool Clean { get; set; }

        public ICollection<PublishDirFileInfo> PublishFiles { get; set; }
    }
}