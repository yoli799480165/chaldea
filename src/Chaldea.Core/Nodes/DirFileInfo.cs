using System;

namespace Chaldea.Core.Nodes
{
    public enum DirFileType
    {
        Directory = 0,
        File = 1
    }

    public class DirFileInfo
    {
        public DirFileType Type { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public DateTime ModifyTime { get; set; }
        public string Size { get; set; }
    }
}