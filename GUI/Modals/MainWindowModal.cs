using System.ComponentModel;


namespace GUI
{
    class MainWindowModal : IMaimWindowModal
    {
        /// <summary>
        /// implement the interface
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        private bool connected;
        private GuiClient client;

        /// <summary>
        /// constructor - initalized if client connected to server (in not conneceted = false)
        /// </summary>
        public MainWindowModal()
        {
            client = GuiClient.GetInstance;
            this.connected = client.IsConnected;

        }

        /// <summary>
        /// property - implements the interface
        /// </summary>
        public bool Connected
        {
            get => connected;
            set
            {
                if (connected != value)
                {
                    connected = value;
                    NotifyPropertyChanged("Connected");

                }
            }
        }
        /// <summary>
        /// when the property changes all listeners will get an update
        /// </summary>
        /// <param name="propName"></param>
        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

    }

}
