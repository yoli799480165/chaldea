using System.Collections.Generic;
using Chaldea.Core.Nodes;

namespace Chaldea.Services.Nodes.Dto
{
    public class ExtractFileDto
    {
        public ICollection<DirFileInfo> Files { get; set; }

        public string DestDir { get; set; }

        public string Password { get; set; }
    }
}