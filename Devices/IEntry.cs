using System;
using System.Collections.Generic;
using System.IO;

namespace Devices
{
    public interface IEntry
    {
        string Name { get; }
        string FullName { get; }
        string Link { get; }
        EntryType Type { get; }
        long Size { get; }
        DateTime? Date { get; }
        bool IsDirectory { get; }
        IDevice Device { get; }

        IEnumerable<IEntry> GetFolders();

        IEnumerable<IEntry> GetEntries();

        void CreateFolder(string folderName);

        void CreateLink(string linkName, string linkPath);

        void Delete();

        void Pull(string path, Stream stream);

        void Push(Stream stream, string path);
        
        bool CanDelete { get; }
        bool CanCreateFolder { get; }
        bool CanCreateLink { get; }

    }
}
