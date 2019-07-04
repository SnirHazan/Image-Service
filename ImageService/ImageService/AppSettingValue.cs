using System.Configuration;

namespace ImageService
{
    /// <summary>
    /// The class the save all the configuration settings that appear in the app.config.
    /// </summary>
    class AppSettingValue
    {
        private static string sourceName;
        private static string logName;
        private static string outputDir;
        private static string thumbnailSize;
        private static string handlers;

        /// <summary>
        /// SourceName property.
        /// </summary>
        public static string SourceName
        {
            get
            {
                if(sourceName == null)
                {
                    sourceName = ConfigurationManager.AppSettings.Get("SourceName");
                }
                return sourceName;
            }
            set
            {
                sourceName = value;
            }
        }
        /// <summary>
        /// OutputDir property.
        /// </summary>
        public static string OutputDir
        {
            get
            {
                if (outputDir == null)
                {
                    outputDir = ConfigurationManager.AppSettings.Get("OutputDir");

                }
                return outputDir;
            }
            set
            {
                outputDir = value;
            }
        }
        /// <summary>
        /// LogName property.
        /// </summary>
        public static string LogName
        {
            get
            {
                if (logName == null)
                {
                    logName = ConfigurationManager.AppSettings.Get("LogName");

                }
                return logName;
            }
            set
            {
                logName = value;
            }
        }
        /// <summary>
        /// ThumbnailSize property.
        /// </summary>
        public static string ThumbnailSize
        {
            get
            {
                if (thumbnailSize == null)
                {
                    thumbnailSize = ConfigurationManager.AppSettings.Get("ThumbnailSize");

                }
                return thumbnailSize;
            }
            set
            {
                thumbnailSize = value;
            }
        }
        /// <summary>
        /// Handler property.
        /// </summary>
        public static string Handlers
        {
            get
            {
                if (handlers == null)
                {
                    handlers = ConfigurationManager.AppSettings.Get("Handler");
                }
                return handlers;
            }
            set
            {
                handlers = value;
            }
        }
    }
}
