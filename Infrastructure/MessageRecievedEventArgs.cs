using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    /// <summary>
    /// arguments of Message Recived.
    /// </summary>
    public class MessageRecievedEventArgs
    {
        /// <summary>
        /// The Status Property.
        /// </summary>
        public MessageTypeEnum Status { get; set; }
        /// <summary>
        /// The message Property.
        /// </summary>
        public string Message { get; set; }
    }
}
