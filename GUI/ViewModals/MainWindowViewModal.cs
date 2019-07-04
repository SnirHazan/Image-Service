using System.ComponentModel;

namespace GUI
{
    /// <summary>
    /// this is vm, part ov mvvm, this part make the connection cetween the model and the view
    /// </summary>
	class MainWindowViewModal : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private IMaimWindowModal model;

        /// <summary>
        /// constructor, use the event to notify that property changed
        /// </summary>
        /// <param name="Mymodal">model of mainWindow</param>
		public MainWindowViewModal(IMaimWindowModal Mymodal)
		{
			model = Mymodal;
			model.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
			{
				NotifyPropertyChanged("VM_" + e.PropertyName);
			};
		}
        /// <summary>
        /// notify all lisetenrers that property has changed
        /// </summary>
        /// <param name="propName">name of property that has changed</param>
		public void NotifyPropertyChanged(string propName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
		}
        /// <summary>
        /// property of connection server-(TCP)client
        /// </summary>
		public bool VM_Connected
		{
			get => model.Connected;
		}
	}
}
