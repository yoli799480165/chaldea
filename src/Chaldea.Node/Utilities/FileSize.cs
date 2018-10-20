using System;
using System.Collections.Generic;
using System.Text;

namespace Chaldea.Node.Utilities
{
    public class FileHelper
    {
        public static string GetSize(double size)
        {
            var units = new[] { "B", "KB", "MB", "GB", "TB", "PB" };
            var mod = 1024.0;
            var i = 0;
            while (size >= mod)
            {
                size /= mod;
                i++;
            }

            return Math.Round(size) + units[i];
        }
    }
}
