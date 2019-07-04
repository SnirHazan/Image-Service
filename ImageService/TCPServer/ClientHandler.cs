using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Infrastructure;
using Newtonsoft.Json;

namespace ImageService
{
    /// <summary>
    /// ClientHandler class
    /// </summary>
    class ClientHandler : IClientHandler
    {
        private IImageController imageController;
        private ILoggingModel loggingModel;
        public static Mutex m = new Mutex();
        // The event that called when closeHandler commad was send from the client to the server.
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;

        /// <summary>
        /// constructor of ClientHandler
        /// </summary>
        /// <param name="ic"> Image controller</param>
        /// <param name="lm">IloggingModel</param>
        public ClientHandler(IImageController ic, ILoggingModel lm)
        {
            imageController = ic;
            loggingModel = lm;
           
        }
        /// <summary>
        /// create handler to specific client - read from the client command, use the controller to 
        /// execute it and write to the client the answer.
        /// </summary>
        /// <param name="client">specific tcpClient to handle</param>
        public void HandleClient(TcpClient client)
        {
            using (NetworkStream stream = client.GetStream())
            using (BinaryReader reader = new BinaryReader(stream))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {


                while (true)
                {
                    string newcommand = reader.ReadString();
                    CommandRecievedEventArgs command = JsonConvert.DeserializeObject<CommandRecievedEventArgs>(newcommand);
                    if (command.CommandID == (int)CommandStateEnum.CLOSE_HANDLER)
                    {
                        CommandRecieved?.Invoke(this, command);
                        imageController.ExecuteCommand((int)command.CommandID, command.Args, out bool result, out MessageTypeEnum type);

                    }
                    else
                    {
                        string msg = imageController.ExecuteCommand((int)command.CommandID, null, out bool result, out MessageTypeEnum type);
                        m.WaitOne();
                        writer.Write(msg);
                        m.ReleaseMutex();
                    }
                }
            }
        }
    }
}