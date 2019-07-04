using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
	interface IMaimWindowModal: INotifyPropertyChanged
	{
        /// <summary>
        /// property, contains bool, return true if the client is connected ti server, else return false.
        /// this will be incike only in the first time client tries to connect the server (won't do "polling")
        /// </summary>
		bool Connected { get; set; }
	}
}
