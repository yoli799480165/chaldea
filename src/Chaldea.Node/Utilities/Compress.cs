using System;
using System.IO;
using System.Linq;
using SharpCompress.Archives;
using SharpCompress.Archives.Rar;
using SharpCompress.Archives.Zip;
using SharpCompress.Common;
using SharpCompress.Readers;

namespace Chaldea.Node.Utilities
{
    public static class Compress
    {
        public static void Extract(string filePath, string destDir, ReaderOptions readerOptions = null)
        {
            var ext = Path.GetExtension(filePath).ToLower();
            switch (ext)
            {
                case ".zip":
                {
                    using (var archive = ZipArchive.Open(filePath, readerOptions))
                    {
                        foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                            entry.WriteToDirectory(destDir, new ExtractionOptions
                            {
                                ExtractFullPath = true,
                                Overwrite = true
                            });
                    }

                    break;
                }
                case ".rar":
                {
                    using (var archive = RarArchive.Open(filePath, readerOptions))
                    {
                        foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                            entry.WriteToDirectory(destDir, new ExtractionOptions
                            {
                                ExtractFullPath = true,
                                Overwrite = true
                            });
                    }

                    break;
                }
                default:
                {
                    using (Stream stream = File.OpenRead(filePath))
                    using (var reader = ReaderFactory.Open(stream, readerOptions))
                    {
                        while (reader.MoveToNextEntry())
                            if (!reader.Entry.IsDirectory)
                                reader.WriteEntryToDirectory(destDir, new ExtractionOptions
                                {
                                    ExtractFullPath = true,
                                    Overwrite = true
                                });
                    }

                    break;
                }
            }
        }
    }
}