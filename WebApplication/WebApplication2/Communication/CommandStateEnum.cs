
namespace WebApplication2.Communication
{
    /// <summary>
    /// Enum of Command State (to int)
    /// </summary>
    public enum CommandStateEnum : int
    {
        NEW_FILE,
        CLOSE,
        CLOSE_HANDLER,
        GET_APP_CONFIG,
        GET_ALL_LOG,
        GET_NEW_LOG,
    }
}
