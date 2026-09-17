using System;

namespace Framework.Core.Network
{

    public struct NetConnectResult
    {
        public NetConnectResultStatus Status;
        public Exception Exception;
    }
    
    public struct NetSendResult
    {
        public NetSendResultStatus Status;
        public Exception Exception;
    }
    
    public struct NetReceiveResult
    {
        public NetReceiveResultStatus Status;
        public Exception Exception;
        public byte[] Data;
    }
    
}