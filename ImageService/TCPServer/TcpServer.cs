using Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService
{
    /// <summary>
    /// The tcpServer class
    /// </summary>
    internal class TcpServer
    {
        private int port;
        private TcpListener listener;
        private IClientHandler ch;
        private List<TcpClient> allClients;
        public static Mutex writeMutex = ClientHandler.m;

        /// <summary>
        /// Constructor of TcpServer
        /// </summary>
        /// <param name="port"> The pory of the server</param>
        /// <param name="ch">IClientHanlder - the class that handle the client of the server</param>
        public TcpServer(int port, IClientHandler ch)
        {
            this.port = port;
            this.ch = ch;
            //Add the SendAllClientCommand to the CommandRecieved event of the clientHanlder.
            ch.CommandRecieved += SendAllClientCommand;
            this.allClients = new List<TcpClient>();
        }

        /// <summary>
        /// Start to listen to client, accept clients' and send them to handler client function.
        /// </summary>
        public void Start()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            listener = new TcpListener(ep);
            listener.Start();

            while (true)
            {
                try
                {
                    TcpClient client = listener.AcceptTcpClient();
                    this.allClients.Add(client);

                    Task t = new Task(() =>
                    {
                        ch.HandleClient(client);
                    });
                    t.Start();
                }
                catch (SocketException)
                {
                    this.Stop();
                    break;
                }
            }
        }

        /// <summary>
        /// Stop listen to client.
        /// </summary>
        public void Stop()
        {
            listener.Stop();
        }

        /// <summary>
        /// Write new log to all clients.
        /// </summary>
        /// <param name="message">msg to write</param>
        /// <param name="type"> <see cref="MessageTypeEnum"/></param>
        public void NewLogArriaved(object sender, MessageRecievedEventArgs mra)
        {
            string[] argument = new string[2];
            argument[0] = mra.Message;
            argument[1] = ((int)mra.Status).ToString();
            CommandRecievedEventArgs e = new CommandRecievedEventArgs((int)CommandStateEnum.GET_NEW_LOG, argument, "");
            string jCommand = JsonConvert.SerializeObject(e);

            List<TcpClient> connectedClient = new List<TcpClient>(this.allClients);

            try
            {
                foreach (TcpClient client in connectedClient)
                {
                    new Task(() =>
                    {
                        try
                        {
                            NetworkStream clientStream = client.GetStream();
                            BinaryWriter write = new BinaryWriter(clientStream);
                            writeMutex.WaitOne();
                            write.Write(jCommand);
                            writeMutex.ReleaseMutex();
                        }
                        catch (Exception)
                        {
                            this.allClients.Remove(client);
                        }
                    }).Start();
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Send to all the Client to close a specific Handler.
        /// </summary>
        /// <param name="sender">is the class that call this method</param>
        /// <param name="e">save all the arguments to the event </param>
        public void SendAllClientCommand(object sender, CommandRecievedEventArgs e) 
        {
            if(e.CommandID == (int)CommandStateEnum.CLOSE_HANDLER)
            {
                string jCommand = JsonConvert.SerializeObject(e);

                List<TcpClient> connectedClient = new List<TcpClient>(this.allClients);

                try
                {
                    foreach (TcpClient client in connectedClient)
                    {
                        new Task(() =>
                        {
                            try
                            {
                                NetworkStream clientStream = client.GetStream();
                                BinaryWriter write = new BinaryWriter(clientStream);
                                writeMutex.WaitOne();
                                write.Write(jCommand);
                                writeMutex.ReleaseMutex();
                            }
                            catch (Exception)
                            {
                                this.allClients.Remove(client);
                            }
                        }).Start();
                    }
                }
                catch (Exception)
                {

                }

            }

        }

    }
}