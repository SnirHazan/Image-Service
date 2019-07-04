using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Prism.Commands;
using System.Windows.Input;
using Infrastructure;

namespace GUI
{
    /// <summary>
    /// this is vm, part ov mvvm, this part make the connection between the model and the view
    /// </summary>
    class SettingsViewModal : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		private ISettingsModal modal;
		private string selected;
        private GuiClient client;

        public ICommand RemoveHandlerCommand { get; private set; }

        public SettingsViewModal(ISettingsModal modal)
		{
			this.modal = modal;
            this.client = GuiClient.GetInstance;
			this.modal.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
			{
				NotifyPropertyChanged("VM_" + e.PropertyName);
			};
            RemoveHandlerCommand = new DelegateCommand<object>(OnRemoveHandler, CanExecute);
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
        /// property of collection, returns the model handlers list
        /// </summary>
		public ObservableCollection<string> VM_HandlerList
		{
			get => modal.HandlerList;
			set => throw new NotImplementedException();
		}

		public string VM_OutputDir
		{
			get => modal.OutputDir;
		}

		public string VM_SourceName
		{
			get => modal.SourceName;
		}

		public string VM_LogName
		{
			get => modal.LogName;
		}

		public string VM_ThumbnailSize
		{
			get => modal.ThumbnailSize;
		}

        /// <summary>
        /// the seclected handler to remove
        /// </summary>
		public string Selected
		{
			get => selected;

			set
			{
				selected = value;
                var command = RemoveHandlerCommand as DelegateCommand<object>;
                command.RaiseCanExecuteChanged();
            }
		}
        /// <summary>
        /// when hit the button - send command to server to notify him
        /// and execute the rutine for this command
        /// </summary>
        /// <param name="obj"></param>
        private void OnRemoveHandler(object obj)
        {
            string[] argument = new string[1];
            argument[0] = Selected;
            CommandRecievedEventArgs sendCommand = new CommandRecievedEventArgs((int)CommandStateEnum.CLOSE_HANDLER, argument,"");
            client.SendCommandToServer(sendCommand);
        }
        /// <summary>
        /// can execute (btn) only if handler selected before
        /// </summary>
        /// <param name="obj">button</param>
        /// <returns></returns>
        private bool CanExecute(object obj)
        {
            if (Selected != null)
                return true;
            return false;
        }
    }
}
