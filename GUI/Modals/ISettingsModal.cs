using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
    interface ISettingsModal: INotifyPropertyChanged
	{
        /// <summary>
        /// property of observable collection - will be bind to the view and edit it.
        /// </summary>
		ObservableCollection<string> HandlerList { get; set; }
        /// <summary>
        /// property - containd the value (string) of outputdir
        /// </summary>
		string OutputDir { get; set; }
        /// <summary>
        /// property - containd the value (string) of sourcename
        /// </summary>
        string SourceName { get; set; }
        /// <summary>
        /// property - containd the value (string) of logname
        /// </summary>
        string LogName { get; set; }
        /// <summary>
        /// property - containd the value (string) of size of thumbnails images
        /// </summary>
		string ThumbnailSize { get; set; }
	}
}
