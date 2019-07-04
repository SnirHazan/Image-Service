using System.Text;
using Infrastructure;

namespace ImageService
{
    /// <summary>
    /// Close handle for directory
    /// </summary>
    class CloseHandleCommand : ICommand
    {
        private ImageServer severImage;
        /// <summary>
        /// constructor of CloseHandleCommand
        /// </summary>
        /// <param name="server">ImageSever</param>
        public CloseHandleCommand(ImageServer server)
        {
            severImage = server;
        }
        /// <summary>
        /// execute CloseHandleCommand.
        /// </summary>
        /// <param name="args"> arguments for command</param>
        /// <param name="result">true if success, otherwise false</param>
        /// <param name="type">{info,warning,fail} according to execute</param>
        /// <returns>path if success, else error messege  </returns>
        public string Execute(string[] args, out bool result, out MessageTypeEnum type)
        {
            result = true;
            type = MessageTypeEnum.INFO;
            CommandRecievedEventArgs returnCommand = new CommandRecievedEventArgs((int)CommandStateEnum.CLOSE_HANDLER, args, null);
            //remove the handler from the setting Configuration.
            
            string[] handlers = AppSettingValue.Handlers.Split(';');
            StringBuilder sb = new StringBuilder();
            foreach (string path in handlers)
            {
                if (path == args[0])
                    continue;
                sb.Append(path).Append(';');
            }
            if (sb.Length != 0)
                sb.Length--;

            AppSettingValue.Handlers = sb.ToString();
            CommandRecievedEventArgs e = new CommandRecievedEventArgs((int)CommandStateEnum.CLOSE, null, args[0]);

            this.severImage.SendCommand(e);

            return string.Empty;
        }
    }
}
