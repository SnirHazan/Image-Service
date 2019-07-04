using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;


namespace ImageService
{
    /// <summary>
    /// Logging model interface
    /// </summary>
    interface ILoggingModel
    {
        /// <summary>
        /// The event that notifies about a new message being recieved
        /// </summary>
        event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        /// <summary>
        /// write msg to Log event
        /// </summary>
        /// <param name="message">msg to write</param>
        /// <param name="type"> <see cref="MessageTypeEnum"/></param>
       void Log(string message, MessageTypeEnum type);
        ObservableCollection<MessageRecievedEventArgs> LogMsg { get; set; }

    }
}
