using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Infrastructure;
using Newtonsoft.Json;

namespace GUI
{
    /// <summary>
    /// GuiClient singelton Class.
    /// </summary>
    class GuiClient
    {
        private bool isConnected = false;
        public static Mutex writeMutex = new Mutex();
        private static GuiClient gui;
        private TcpClient client;
        //The event that called when new command recieved from the server.
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;
        /// <summary>
        /// Private constructor of GuiClient. 
        /// </summary>
        private GuiClient()
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
                IsConnected = true;
            }
            catch (Exception)
            {
                IsConnected = false;
            }
        }
        /// <summary>
        /// IsConnected Property - tell us of the Guiclient is conncted to the server.
        /// </summary>
        /// <returns>True if connect, else false </returns>

        public bool IsConnected
        {
            get => this.isConnected;
            set => this.isConnected = value;
        }
        /// <summary>
        /// Return instance of the GuiClient, if it's NULL - activate the private constructor.
        /// </summary>
        /// <returns>Instance of GuiClient - Singelton class</returns>

        public static GuiClient GetInstance
        {
            get
            {
                if (gui == null)
                {
                    gui = new GuiClient();
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
                string result = string.Empty;
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
