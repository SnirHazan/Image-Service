using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Runtime.InteropServices;
using System.Configuration;
using Infrastructure;
using System.Threading.Tasks;

public enum ServiceState
{
    SERVICE_STOPPED = 0x00000001,
    SERVICE_START_PENDING = 0x00000002,
    SERVICE_STOP_PENDING = 0x00000003,
    SERVICE_RUNNING = 0x00000004,
    SERVICE_CONTINUE_PENDING = 0x00000005,
    SERVICE_PAUSE_PENDING = 0x00000006,
    SERVICE_PAUSED = 0x00000007,
}

[StructLayout(LayoutKind.Sequential)]
public struct ServiceStatus
{
    public int dwServiceType;
    public ServiceState dwCurrentState;
    public int dwControlsAccepted;
    public int dwWin32ExitCode;
    public int dwServiceSpecificExitCode;
    public int dwCheckPoint;
    public int dwWaitHint;
};

namespace ImageService
{
    /// <summary>
    /// Image Service
    /// </summary>
    public partial class ImageService : ServiceBase
    {
        private ILoggingModel m_logging;
        private IImageController c_controller;
        private IImageModel m_imageService;
        private ImageServer server;
        private TcpServer tcpServer;
        private string eventSourceName;
        private string logName;

        /// <summary>
        /// constructor - init logger, modal,server,controller
        /// </summary>
        /// <param name="args">Not used</param>
        public ImageService(string[] args)
        {
            InitializeComponent();

             eventSourceName = ConfigurationManager.AppSettings.Get("SourceName");
             logName = ConfigurationManager.AppSettings.Get("LogName");



            eventLog1 = new System.Diagnostics.EventLog();


            if (!System.Diagnostics.EventLog.SourceExists(eventSourceName))
            {
                System.Diagnostics.EventLog.CreateEventSource(eventSourceName, logName);
            }
            eventLog1.Source = eventSourceName;
            eventLog1.Log = logName;
            m_logging = new LoggingModel(eventLog1);

            m_imageService = new ImageModel();
            c_controller = new ImageController(m_imageService);
            server = new ImageServer(c_controller, m_logging);

            IClientHandler ch = new ClientHandler(new ServerController(m_logging,this.server), this.m_logging);
            tcpServer = new TcpServer(8005, ch);

            m_logging.MessageRecieved += OnMsg;
            m_logging.MessageRecieved += tcpServer.NewLogArriaved;
            

            Task t = new Task(() =>
            {
                tcpServer.Start();
            });
            t.Start();



        }
        /// <summary>
        /// activite when service start
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            // Update the service state to Start Pending.  
            ServiceStatus serviceStatus = new ServiceStatus
            {
                dwCurrentState = ServiceState.SERVICE_START_PENDING,
                dwWaitHint = 100000
            };
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            //m_logging.Log("In OnStart", MessageTypeEnum.INFO);
            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }
        /// <summary>
        /// when the MesseageRecived event invoked- do this:
        /// write to logger the message and type
        /// </summary>
        /// <param name="src">sender</param>
        /// <param name="mra">arguments of messegeRecive <seealso cref="MessageRecievedEventArgs"/></param>
        private void OnMsg(object src, MessageRecievedEventArgs mra)
        {
            if(mra.Status == MessageTypeEnum.FAIL)
            {
                eventLog1.WriteEntry(mra.Message,EventLogEntryType.Error);
            } else if (mra.Status == MessageTypeEnum.INFO)
            {
                eventLog1.WriteEntry(mra.Message, EventLogEntryType.Information);
            } else
            {
                eventLog1.WriteEntry(mra.Message, EventLogEntryType.Warning);
            }

        }
        /// <summary>
        /// when stop the service, send clode command to all handlers 
        /// </summary>
        protected override void OnStop()
        {
            m_logging.Log("StartCloseService", MessageTypeEnum.INFO);

            CommandRecievedEventArgs e = new CommandRecievedEventArgs((int)CommandStateEnum.CLOSE, null, "*");
            server.SendCommand(e);
            
            m_logging.Log("Service Stopped", MessageTypeEnum.INFO);
            EventLog.DeleteEventSource(eventSourceName);
            EventLog.Delete(logName);
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);
    }
}
