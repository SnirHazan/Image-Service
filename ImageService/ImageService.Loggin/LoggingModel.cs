using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using Infrastructure;

namespace ImageService
{
    /// <summary>
    /// Logging model
    /// </summary>
    class LoggingModel : ILoggingModel
    {
        private EventLog imageServiceEventLog;
        private ObservableCollection<MessageRecievedEventArgs> logMsg;

        public LoggingModel(EventLog log)
        {
            this.imageServiceEventLog = log;
            this.logMsg = new ObservableCollection<MessageRecievedEventArgs>();
            GetAllLogMsg(log);
        }
        /// <summary>
        /// The event that notifies about a new message being recieved
        /// </summary>
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        /// <summary>
        /// write msg to Log event
        /// </summary>
        /// <param name="message">msg to write</param>
        /// <param name="type"> <see cref="MessageTypeEnum"/></param>
        public void Log(string message, MessageTypeEnum type)
        {
            MessageRecievedEventArgs msg = new MessageRecievedEventArgs
            {
                Message = message,
                Status = type
            };
            this.logMsg.Add(msg);
            Thread.Sleep(millisecondsTimeout: 500);
            MessageRecieved?.Invoke(this, msg);
        }


        public ObservableCollection<MessageRecievedEventArgs> LogMsg
        {
            get => this.logMsg;
            set => throw new NotImplementedException();
        }

        private void GetAllLogMsg(EventLog log)
        {
            EventLogEntry[] logEvent = new EventLogEntry[log.Entries.Count];
            log.Entries.CopyTo(logEvent, 0);
            foreach (EventLogEntry e in logEvent)
            {
                MessageTypeEnum type = GetTypeOfMsg(e.EntryType);
                MessageRecievedEventArgs msg = new MessageRecievedEventArgs { Message = e.Message, Status = type };
                this.logMsg.Add(msg);
            }

        }

        public static MessageTypeEnum GetTypeOfMsg(EventLogEntryType entryType)
        {
            if (entryType == EventLogEntryType.Information)
                return MessageTypeEnum.INFO;
            else if (entryType == EventLogEntryType.Warning)
                return MessageTypeEnum.WARNING;
            else
                return MessageTypeEnum.FAIL;
        }

    }
}