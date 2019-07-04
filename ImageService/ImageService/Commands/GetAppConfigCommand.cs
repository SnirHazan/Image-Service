using Infrastructure;
using Newtonsoft.Json;

namespace ImageService
{
    /// <summary>
    /// GetApp Config command implements ICommand
    /// </summary>
    class GetAppConfigCommand : ICommand
    {
        /// <summary>
        /// execute GetAppConfig Command.
        /// </summary>
        /// <param name="args"> arguments for command</param>
        /// <param name="result">true if success, otherwise false</param>
        /// <param name="type">{info,warning,fail} according to execute</param>
        /// <returns>GetAllConfig command - Json - back to client</returns>
        public string Execute(string[] args, out bool result, out MessageTypeEnum type)
        {

            string[] settings = new string[5];
            settings[0] = AppSettingValue.OutputDir;
            settings[1] = AppSettingValue.SourceName;
            settings[2] = AppSettingValue.LogName;
            settings[3] = AppSettingValue.ThumbnailSize;
            settings[4] = AppSettingValue.Handlers;

            result = true;
            type = MessageTypeEnum.INFO;
            CommandRecievedEventArgs returnCommand = new CommandRecievedEventArgs((int)CommandStateEnum.GET_APP_CONFIG, settings,null);
            string retCommnad = JsonConvert.SerializeObject(returnCommand);
            return retCommnad;
        }
    }
}
