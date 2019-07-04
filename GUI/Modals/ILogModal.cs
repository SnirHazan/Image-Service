using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

namespace GUI
{
    interface ILogModal : INotifyPropertyChanged
    {
        /// <summary>
        /// property of observable collection - will be bind to the view and edit it.
        /// </summary>
        ObservableCollection<MessageRecievedEventArgs> LogList { get; set; }
        
    }
}
