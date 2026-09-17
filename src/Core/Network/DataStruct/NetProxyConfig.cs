namespace Framework.Core.Network
{
    public struct NetProxyConfig
    {
        public bool UseProxy;
        
        public string Host { get; set; }
        public int Port { get; set; }
        
        public string UserName;
        public string Password;

        public bool BypassOnLocal;
    }
}