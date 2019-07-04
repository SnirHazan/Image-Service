using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using Infrastructure;
using Newtonsoft.Json;

namespace GUI
{
    class LogModel : ILogModal
    {
        private GuiClient client;
        /// <summary>
        /// property of log collection - bind to view.
        /// </summary>
        public ObservableCollection<MessageRecievedEventArgs> LogList { get; set; }
        /// <summary>
        /// event, notify if property has changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// notyfy that property has changed
        /// </summary>
        /// <param name="propName"> the property that has changed - and needed to be notifyed</param>
        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        /// <summary>
        /// construtor - when created - ask from server the log list
        /// </summary>
        public LogModel()
        {
            //singelton
            client = GuiClient.GetInstance;
            client.CommandRecieved += this.OnUpdate;
            InitializedLog();
        }

        /// <summary>
        /// this method is for the binding between the observable collection and the view
        /// its try to ask the server the log list and add them to the collection
        /// </summary>
        private void InitializedLog()
        {
            LogList = new ObservableCollection<MessageRecievedEventArgs>();
            Object thisLock = new Object();
            BindingOperations.EnableCollectionSynchronization(LogList, thisLock);

            //if client connected to server send this command
            if (this.client.IsConnected)
            {
                CommandRecievedEventArgs commanToSent = new CommandRecievedEventArgs((int)CommandStateEnum.GET_ALL_LOG, new string[5], "");
                client.SendCommandToServer(commanToSent);
            }
            else //client not connected - log list write this to the log
            {
                MessageRecievedEventArgs e = new MessageRecievedEventArgs
                {
                    Message = "Failed to connect to the server",
                    Status = MessageTypeEnum.FAIL
                };
                LogList.Add(e);
            }
        }

        /// <summary>
        /// when update occure find the reason and update all clients with this update
        /// </summary>
        /// <param name="src">the object send this</param>
        /// <param name="e">the structure of caommand recivedeventargs contains CommandId </param>
        private void OnUpdate(object src, CommandRecievedEventArgs e)
        {
            if (e.CommandID == (int)CommandStateEnum.GET_ALL_LOG)
                this.AllLogArraived(e);
            else if (e.CommandID == (int)CommandStateEnum.GET_NEW_LOG)
                this.NewLogArraived(e);
        }
        /// <summary>
        /// update clients by add this update to thr LogList
        /// </summary>
        /// <param name="e">the structure of caommand recivedeventargs contains CommandId and more</param>
        private void NewLogArraived(CommandRecievedEventArgs e)
        {
            MessageRecievedEventArgs newLog = new MessageRecievedEventArgs
            {
                Message = e.Args[0]
            };
            int num = Int32.Parse(e.Args[1]);
            //get the type of msg
            switch (num)
            {
                case 0:
                    newLog.Status = MessageTypeEnum.INFO;
                    break;
                case 1:
                    newLog.Status = MessageTypeEnum.WARNING;
                    break;
                case 2:
                    newLog.Status = MessageTypeEnum.FAIL;
                    break;
            }
            this.LogList.Add(newLog);

        }
        /// <summary>
        /// update clients by add all logs to LogList (used for initalized the LogList - in the constructor)
        /// </summary>
        /// <param name="e">the structure of caommand recivedeventargs contains CommandId</param>
        private void AllLogArraived(CommandRecievedEventArgs e)
        {
            ObservableCollection<MessageRecievedEventArgs> newOne = JsonConvert.DeserializeObject<ObservableCollection<MessageRecievedEventArgs>>(e.Args[0]);
            foreach (MessageRecievedEventArgs msg in newOne)
            {
                this.LogList.Add(msg);
            }
        }
    }
}
