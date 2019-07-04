using System.Collections.Generic;
using Infrastructure;


namespace ImageService
{
    /// <summary>
    /// Image Controller
    /// </summary>
    class ServerController : IImageController
    {
        private Dictionary<int, ICommand> commands;
        /*
        private IImageModel m_imageModel;
        */
        private ILoggingModel m_logModal;
        private ImageServer sever;

        /// <summary>
        /// cons't
        /// </summary>
        /// <param name="logModal">ILoggingModal</param>
        /// <param name="server"> ImageServer</param>
        public ServerController(ILoggingModel logModal, ImageServer server)
        {
            //Dictionary: key - CommandState , value - command to execute
            this.commands = new Dictionary<int, ICommand>();
            this.m_logModal = logModal;
            this.sever = server;

            //Get App Config command
            commands[(int)CommandStateEnum.GET_APP_CONFIG] = new GetAppConfigCommand();

            //Get All Log command
            commands[(int)CommandStateEnum.GET_ALL_LOG] = new GetLogCommand(this.m_logModal);

            //Close Handler command
            commands[(int)CommandStateEnum.CLOSE_HANDLER] = new CloseHandleCommand(this.sever);

        }
        /// <summary>
        /// Executing the Command Requet
        /// </summary>
        /// <param name="commandID">CommandId - <see cref="CommandState"/></param>
        /// <param name="args">arguments for the command</param>
        /// <param name="result"> true if succeeded</param>
        /// <param name="type"> <seealso cref="MessageTypeEnum"/></param>
        /// <returns>string - msg of the execute </returns>
        public string ExecuteCommand(int commandID, string[] args, out bool result, out MessageTypeEnum type)
        {
           return commands[commandID].Execute(args, out result,out type);
        }
    }
}
