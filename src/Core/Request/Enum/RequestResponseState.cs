namespace Framework.Core.Request
{
    public enum RequestResponseState
    {
        Unfinished,
        NetworkError,
        Timeout,
        ServerError,
        ClientError,
        DataError,
        Success,
    }
}