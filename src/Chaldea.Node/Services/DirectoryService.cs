using System.Collections.Generic;
using System.IO;
using Chaldea.Node.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Chaldea.Node.Services
{
    public class DirectoryService
    {
        private readonly ILogger<DirectoryService> _logger;
        private readonly IOptions<Appsettings> _options;

        public DirectoryService(
            ILogger<DirectoryService> logger,
            IOptions<Appsettings> options)
        {
            _logger = logger;
            _options = options;
        }

        public ICollection<string> GetRootDirs()
        {
            return _options.Value.RootDirs;
        }

        public ICollection<string> GetDirFiles(string path)
        {
            var list = new List<string>();
            var dirs = Directory.GetDirectories(path);
            var files = Directory.GetFiles(path);
            list.AddRange(dirs);
            list.AddRange(files);
            return list;
        }
    }
}