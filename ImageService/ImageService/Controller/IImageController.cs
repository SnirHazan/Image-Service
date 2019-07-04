using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

namespace ImageService
{
    /// <summary>
    /// Interface of Image Controller
    /// </summary>
    interface IImageController
    {
        /// <summary>
        /// Executing the Command Requet
        /// </summary>
        /// <param name="commandID">CommandId - <see cref="CommandState"/></param>
        /// <param name="args">arguments for the command</param>
        /// <param name="result"> true if succeeded</param>
        /// <param name="type"> <seealso cref="MessageTypeEnum"/></param>
        /// <returns>string - msg of the execute </returns>
        string ExecuteCommand(int commandID, string[] args, out bool result,out MessageTypeEnum type);// 
    }
}
