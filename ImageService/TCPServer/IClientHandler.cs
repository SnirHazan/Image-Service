using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
{ /// <summary>
  /// IClientHandler interface
  /// </summary>
    interface IClientHandler
    {
        /// <summary>
        /// create handler to specific client - read from the client command, use the controller to 
        /// execute it and write to the client the answer.
        /// </summary>
        /// <param name="client">specific tcpClient to handle</param>
        void HandleClient(TcpClient client);

        // The event that called when closeHandler commad was send from the client to the server.
        event EventHandler<CommandRecievedEventArgs> CommandRecieved;

    }
}
