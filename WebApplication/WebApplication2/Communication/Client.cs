using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace WebApplication2.Communication
{
    class Client
    {
        public static Mutex writeMutex = new Mutex();
        private static Client gui;
        private TcpClient client;
        //The event that called when new command recieved from the server.
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;
        /// <summary>
        /// Private constructor of GuiClient. 
        /// </summary>
        private Client()
        {
            ConnectClientToServer();

        }

        /// <summary>
        /// This method try to cinnect the Guiclient to the server.
        /// </summary>
        private void ConnectClientToServer()
        {
            try
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8005);
                client = new TcpClient();
                client.Connect(ep);
                Console.WriteLine("true");
                IsConnected = true;
            }
            catch (Exception)
            {
                Console.WriteLine("False");
                IsConnected = false;
            }
        }

        public bool IsConnected { get; set; }

        public void IsClientConnected()
        {
            IsConnected = client.Connected;
        }


        /// <summary>
        /// Return instance of the GuiClient, if it's NULL - activate the private constructor.
        /// </summary>
        /// <returns>Instance of GuiClient - Singelton class</returns>

        public static Client GetInstance
        {
            get
            {
                if (gui == null)
                {
                    gui = new Client();
                }
                return gui;
            }
        }
        /// <summary>
        /// This function send commands to the server.
        /// </summary>
        /// <param name="e"> CommandRecievedEventArgs - the args of the Command</param>
        public void SendCommandToServer(CommandRecievedEventArgs e)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                BinaryWriter writer = new BinaryWriter(stream);
                BinaryReader reader = new BinaryReader(stream);
                new Task(() =>
                {
                    try
                    {
                        string jComman = JsonConvert.SerializeObject(e);
                        writeMutex.WaitOne();
                        writer.Write(jComman);
                        writeMutex.ReleaseMutex();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }).Start();
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// This function recieve commands from the server, and call to CommandRecieved Event.
        /// </summary>
        public void RecivedMessageFromServer()
        {
            try
            {
                string s = null;
                NetworkStream stream = client.GetStream();
                BinaryReader reader = new BinaryReader(stream);
                new Task(() =>
                {
                    while (true)
                    {

                        try
                        {
                            s = reader.ReadString();
                            stream.Flush();
                            CommandRecievedEventArgs newComman = JsonConvert.DeserializeObject<CommandRecievedEventArgs>(s);
                            this.CommandRecieved?.Invoke(this, newComman);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }

                    }
                }).Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}

