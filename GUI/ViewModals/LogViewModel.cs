using System.Collections.ObjectModel;
using System.ComponentModel;
using Infrastructure;
using System;

namespace GUI
{
    class LogViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        private ILogModal model;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="Mymodal">log modal, so we can use the property LogList</param>
        public LogViewModel(ILogModal Mymodal)
        {
            model = Mymodal;            
        }

        /// <summary>
        /// notify all listeners that this property has changed
        /// </summary>
        /// <param name="propName">the property name (who changed)</param>
		public void NotifyPropertyChanged(string propName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
		}

		public ObservableCollection<MessageRecievedEventArgs> VM_LogList
        {
            get => this.model.LogList;
            set => throw new NotImplementedException();
    }
            

    }
}
