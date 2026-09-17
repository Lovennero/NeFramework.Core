namespace Framework.Core.Request
{
    public struct RequestResponse
    {
        public readonly int StatusCode;
        public readonly RequestResponseState State;
        public readonly byte[] Data;
        public readonly string Error;
        
        public RequestResponse(
            int statusCode,
            RequestResponseState state,
            byte[] data,
            string error)
        {
            StatusCode = statusCode;
            State = state;
            Data = data;
            Error = error;
        }
    }
}