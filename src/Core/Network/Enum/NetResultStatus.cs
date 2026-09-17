namespace Framework.Core.Network
{
    public enum NetConnectResultStatus
    {
        Success, 
        Error,
        Timeout, 
        Cancelled,
        Other
    }
    
    public enum NetSendResultStatus
    {
        Success, 
        Error,
        Cancelled,
        Other
    }

    public enum NetReceiveResultStatus
    {
        Success,
        Cancelled,
        Error,
        Other
    }
}