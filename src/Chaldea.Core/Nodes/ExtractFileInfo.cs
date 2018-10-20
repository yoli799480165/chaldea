using System.Collections.Generic;

namespace Chaldea.Core.Nodes
{
    public class ExtractFileInfo
    {
        public ICollection<DirFileInfo> Files { get; set; }

        public string DestDir { get; set; }

        public string Password { get; set; }
    }
}