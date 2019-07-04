using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
{
    /// <summary>
    /// intreface for handle directories
    /// </summary>
    interface IDirectoryHandler
    {
        /// <summary>
        /// The Event That Notifies that the Directory is being closed
        /// </summary>
        event EventHandler<DirectoryCloseEventArgs> DirectoryClose;
        /// <summary>
        /// Start handle Directory of specific path.
        /// </summary>
        /// <param name="dirPath"> path to start handle</param>
        void StartHandleDirectory(string dirPath);
        /// <summary>
        /// The Event that will be activated upon new Command
        /// </summary>
        /// <param name="sender">is the class that call this method</param>
        /// <param name="e">save all the arguments to the event </param>
        void OnCommandRecieved(object sender, CommandRecievedEventArgs e);
        /// <summary>
        /// The method that will be activated when Close Command activite
        /// </summary>
        void StopHandleDirectory();
    }
}
