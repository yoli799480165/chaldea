﻿using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace BaiduNetDisk
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            BaiduNetDisk.GetDirFiles("/test");
            Console.ReadLine();
        }
    }

    public static class BaiduNetDisk
    {
        private static Process GetProcess(string cmd)
        {
            return new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/opt/BaiduPCS/BaiduPCS-Go",
                    Arguments = cmd,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
        }

        public static string Run(string cmd)
        {
            var process = GetProcess(cmd);
            process.Start();
            var result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }

        public static void GetDirFiles(string path = null)
        {
            var regex =
                @"\s{2}(\d)+\s{4,}(\w+\.?\w+|-)\s{2,}(\d{4}-\d{1,2}-\d{1,2}\s\d{2}:\d{2}:\d{2})\s{2,}(\w+.?\w+)";

            // var result = string.IsNullOrEmpty(path) ? Run("ls") : Run($"ls {path}");
            var result = File.ReadAllText("2.txt");
            var matches = Regex.Matches(result, regex);
            foreach (Match match in matches)
            {
                var groups = match.Groups;
                if (groups.Count > 1)
                {
                    var index = groups[1];
                    var size = groups[2];
                    var modifyTime = groups[3];
                    var name = groups[4];
                    Console.WriteLine($"{index}\t{size}\t{modifyTime}\t{name}");
                }
            }
//            File.WriteAllText("test.txt", result);
//            Console.WriteLine("End");
        }
    }
}