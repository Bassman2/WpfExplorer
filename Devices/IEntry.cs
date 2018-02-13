using ExplorerCtrl;

namespace Devices
{
    public interface IEntry : IExplorerItem
    {
        /// <summary>
        /// Enable create folder in property menu if true
        /// </summary>
        bool CanCreateFolder { get; }

        /// <summary>
        /// Enable create link in property menu if true
        /// </summary>
        bool CanCreateLink { get; }

        /// <summary>
        /// Enable delete in property menu if true
        /// </summary>
        bool CanDelete { get; }

        /// <summary>
        /// Enable rename in property menu if true
        /// </summary>
        bool CanRename { get; }

        /// <summary>
        /// Create a new link
        /// </summary>
        /// <param name="linkName">Name of the link</param>
        /// <param name="linkPath">Reference path</param>
        void CreateLink(string linkName, string linkPath);

        /// <summary>
        /// Delete this item
        /// </summary>
        void Delete();

        /// <summary>
        /// Rename this item
        /// </summary>
        /// <param name="newName">New name of the item</param>
        void Rename(string newName);
    }
}
