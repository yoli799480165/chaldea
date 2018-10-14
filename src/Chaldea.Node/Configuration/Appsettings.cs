using System.Collections.Generic;

namespace Chaldea.Node.Configuration
{
    public class Appsettings
    {
        public string NodeServer { get; set; }

        public ICollection<string> RootDirs { get; set; }
    }
}