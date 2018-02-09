using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplorerCtrl
{
    /// <summary>
    /// Represents the event data for the Refresh event.
    /// </summary>
    public class RefreshEventArgs : EventArgs
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="recursive">Set the recursive property.</param>
        public RefreshEventArgs(bool recursive)
        {
            this.Recursive = recursive;
        }

        /// <summary>
        /// Should the refresh be recursive or not.
        /// </summary>
        public bool Recursive { get; private set; }
    }
}
