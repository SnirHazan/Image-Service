using System.Threading;
using Infrastructure;

namespace ImageService
{
    /// <summary>
    /// New file command implements ICommand
    /// </summary>
    class NewFileCommand : ICommand
    {
        private IImageModel m_model;

        /// <summary>
        /// constructor to Command
        /// </summary>
        /// <param name="modal"> imageModel</param>
        public NewFileCommand(IImageModel modal)
        {
            m_model = modal;            // Storing the Modal
        }

        /// <summary>
        /// execute command.
        /// </summary>
        /// <param name="args"> arguments for command</param>
        /// <param name="result">true if success, otherwise false</param>
        /// <param name="type">{info,warning,fail} according to execute</param>
        /// <returns>path if success, else error messege  </returns>
        public string Execute(string[] args, out bool result, out MessageTypeEnum type)
        {
            Thread.Sleep(1000);
            return m_model.AddFile(args[0], out result,out type);

            // The String Will Return the New Path if result = true, and will return the error message
        }
    }
}
