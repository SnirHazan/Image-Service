using System.Collections.ObjectModel;
using Infrastructure;
using Newtonsoft.Json;

namespace ImageService
{/// <summary>
 /// Get all log command implements ICommand.
 /// </summary>
    class GetLogCommand : ICommand
    {
        private ILoggingModel log_Modal;
        /// <summary>
        /// constructor to Command
        /// </summary>
        /// <param name="logModal"> ILoggingModel</param>
        public GetLogCommand(ILoggingModel logModal)
        {
            this.log_Modal = logModal;
        }
        /// <summary>
        /// execute GetAllLOg Command.
        /// </summary>
        /// <param name="args"> arguments for command</param>
        /// <param name="result">true if success, otherwise false</param>
        /// <param name="type">{info,warning,fail} according to execute</param>
        /// <returns>path if success, else error messege  </returns>
        public string Execute(string[] args, out bool result, out MessageTypeEnum type)
        {
            
            ObservableCollection<MessageRecievedEventArgs> logEntry = this.log_Modal.LogMsg;
            string jsonLog = JsonConvert.SerializeObject(logEntry);
            string[] argument = new string[1];
            argument[0] = jsonLog;
            CommandRecievedEventArgs returnCommand = new CommandRecievedEventArgs((int)CommandStateEnum.GET_ALL_LOG, argument, "");
            string retCommand = JsonConvert.SerializeObject(returnCommand);
            result = true;
            type = MessageTypeEnum.INFO;

            return retCommand;
            
            
        }
    }
}
