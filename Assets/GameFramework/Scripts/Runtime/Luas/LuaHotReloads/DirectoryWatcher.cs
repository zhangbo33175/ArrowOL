using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Honor.Runtime
{
    public class DirectoryWatcher
    {
        public DirectoryWatcher(string dirPath, string filter, FileSystemEventHandler handler)
        {
            CreateWatch(dirPath, filter, handler);
        }

        void CreateWatch(string dirPath, string filter, FileSystemEventHandler handler)
        {
            if (!Directory.Exists(dirPath)) return;

            var watcher = new FileSystemWatcher();
            watcher.IncludeSubdirectories = true;
            watcher.Path = dirPath;
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Filter = filter;
            watcher.Changed += handler;
            watcher.EnableRaisingEvents = true;
            watcher.InternalBufferSize = 10240;
        }
    }

}


