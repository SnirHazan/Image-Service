using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
{
    /// <summary>
    /// arguments of close directory event
    /// </summary>
    public class DirectoryCloseEventArgs : EventArgs
    {
        public string DirectoryPath { get; set; }

        public string Message { get; set; }  
        ///<summary>
        /// constructor 
        /// </summary>
        /// <param name="dirPath">Setting the Directory Name</param>
        /// <param name="message">Storing the String, The Message That goes to the logger</param>
        public DirectoryCloseEventArgs(string dirPath, string message)
        {
            DirectoryPath = dirPath;                
            Message = message;                      
        }
    }
}
