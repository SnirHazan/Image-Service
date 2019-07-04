using System;
using System.Collections.Generic;
using System.IO;
using Infrastructure;


namespace ImageService
{
    /// <summary>
    /// Directory handle class - listen to directories
    /// </summary>
    class DirectoyHandler : IDirectoryHandler
    {
        private IImageController c_imageController;
        private ILoggingModel m_loggingModel;
        List<FileSystemWatcher> watchers;
        private string dirPath;
        string[] filters = { "*.bmp", "*.jpg", "*.gif", "*.png"};

        /// <summary>
        /// cons't 
        /// </summary>
        /// <param name="loggingModel"> loggingModal</param>
        /// <param name="imageController">Controller</param>
        /// <param name="path">which dorectory to listen</param>
        public DirectoyHandler(ILoggingModel loggingModel, IImageController imageController, string path)
        {
            c_imageController = imageController;
            m_loggingModel = loggingModel;
            dirPath = path;
            // list of filters to this directory
            watchers = new List<FileSystemWatcher>();
            this.StartHandleDirectory(path);
        }
        /// <summary>
        /// The Event That Notifies that the Directory is being closed
        /// </summary>
        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;
        /// <summary>
        /// The Event that will be activated upon new Command
        /// </summary>
        /// <param name="sender">is the class that call this method</param>
        /// <param name="e">save all the arguments to the event </param>
        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
         //check if the command should execute on this Directory ("*" - for all directories)
         //remember - Close has no execute command
            if(e.RequestDirPath == dirPath || e.RequestDirPath == "*")
            {
                if (e.CommandID == (int)CommandStateEnum.CLOSE) //if Command is Close
                {
                    DirectoryCloseEventArgs e1 = new DirectoryCloseEventArgs(dirPath, "Stop handle Dir: " + dirPath);
                    StopHandleDirectory();
                    DirectoryClose?.Invoke(this, e1);
                }
                else
                {//use the controller to execute
                    //for new File command taks the args[0].
                    string msg = c_imageController.ExecuteCommand(e.CommandID, e.Args, out bool result , out MessageTypeEnum type);

                    //log write the result msg, with the returning type
                        m_loggingModel.Log(msg, type);
                }
            }
        }
        /// <summary>
        /// Start handle Directory of specific path.
        /// </summary>
        /// <param name="dirPath"> path to start handle</param>
        public void StartHandleDirectory(string dirPath)
        {
            //create fileSystemWatcher and for all filters { "*.bmp", "*.jpg", "*.gif", "*.png"}
            foreach (string f in filters)
            {
                FileSystemWatcher w = new FileSystemWatcher(this.dirPath);
                w.Filter = f;
                w.Created += new FileSystemEventHandler(OnImageCreated); //raise event when created file
                w.EnableRaisingEvents = true;
                watchers.Add(w);    //add to list
            }
        }
        /// <summary>
        /// called when event raised (FileSystemWatcher) - and send newFile Command 
        /// </summary>
        /// <param name="e"> the arguments for NewFileCommands</param>
        private void OnImageCreated(object sender,FileSystemEventArgs e)
        {
            string[] args = { e.FullPath };
            CommandRecievedEventArgs cArgs = new CommandRecievedEventArgs((int)CommandStateEnum.NEW_FILE, args, dirPath);
            OnCommandRecieved(this, cArgs); //execute newFile
        }
        /// <summary>
        /// The method that will be activated when Close Command activite
        /// </summary>
        public void StopHandleDirectory()
        {
            //for every filter - remove it from event
            foreach (FileSystemWatcher w in watchers)
            {
                w.EnableRaisingEvents = false;
            }
            watchers.Clear();//clear the list
        }
    }
}
