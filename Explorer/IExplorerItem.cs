using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace ExplorerCtrl
{
    /// <summary>
    /// Interface for the explorer item class.
    /// </summary>
    public interface IExplorerItem : IEquatable<IExplorerItem>
    {
        /// <summary>
        /// Event to signal an refresh.
        /// </summary>
        event EventHandler<RefreshEventArgs> Refresh;

        /// <summary>
        /// Name of the item.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Name with path of the item.
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Link destination path of the item.
        /// </summary>
        string Link { get; }

        /// <summary>
        /// Size of the item in bytes.
        /// </summary>
        long Size { get; }

        /// <summary>
        /// Last changing date of the item.
        /// </summary>
        DateTime? Date { get; }

        /// <summary>
        /// Type of the item. (Folder, File or Link)
        /// </summary>
        ExplorerItemType Type { get; }

        /// <summary>
        /// Icon of the folder. Best in size 16 x 16.
        /// </summary>
        ImageSource Icon { get; }

        /// <summary>
        /// True if folder or link.
        /// </summary>
        bool IsDirectory { get; }

        /// <summary>
        /// True is directory has child elements.
        /// </summary>
        bool HasChildren { get; }

        /// <summary>
        /// Child elements of a directory
        /// </summary>
        IEnumerable<IExplorerItem> Children { get; }

        /// <summary>
        /// Push a stream to a file.
        /// </summary>
        /// <param name="stream">Stream to push.</param>
        /// <param name="path">Path of the file.</param>
        void Push(Stream stream, string path);

        /// <summary>
        /// Pull a path to a stream.
        /// </summary>
        /// <param name="path">Stream to pull</param>
        /// <param name="stream">Path of the file.</param>
        void Pull(string path, Stream stream);

        /// <summary>
        /// Create a new folder
        /// </summary>
        /// <param name="path">Path of the new folder</param>
        void CreateFolder(string path);
    }
}
