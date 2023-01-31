using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplorerCtrl
{
    /// <summary>
    /// Type of the explorer item.
    /// </summary>
    public enum ExplorerItemType
    {
        /// <summary>
        /// The explorer item is a directory.
        /// </summary>
        Directory,

        /// <summary>
        /// The explorer item is a link.
        /// </summary>
        Link,

        /// <summary>
        /// The explorer item is a file.
        /// </summary>
        File
    }
}
