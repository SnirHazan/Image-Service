using System;


namespace Infrastructure

{
    /// <summary>
    /// arguments of Command Recived
    /// </summary>
    public class CommandRecievedEventArgs:EventArgs
    {
        public int CommandID { get; set; }      // The Command ID
        public string[] Args { get; set; }
        public string RequestDirPath { get; set; }  // The Request Directory
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="id"> Command State, dont forget casting to int <seealso cref="CommandState"/></param>
        /// <param name="args">arguments</param>
        /// <param name="path">path of requested directory to execute the command</param>
        public CommandRecievedEventArgs(int id, string[] args, string path)
        {
            CommandID = id;
            Args = args;
            RequestDirPath = path;
        }
    }
}
