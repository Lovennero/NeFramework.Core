namespace Framework.Core.Request
{
    public struct RequestGroupConfig
    {
        // === 并发配置 ===
        public int MaxConcurrent;
        
        // === 超时配置 ===
        public float Timeout;
        
        // === 重试配置 ===
        public int RetryCount;
        public float RetryBaseInterval;
        public float RetryMaxInterval;
        public bool RetryExponential;
        public bool RetryJitter;

        public static RequestGroupConfig Default => new()
        {
            MaxConcurrent = 10, 
            Timeout = 1, 
            RetryCount = 3, 
            RetryBaseInterval = 0.5f, 
            RetryMaxInterval = 1, 
            RetryExponential = true, 
            RetryJitter = true
        };
    }
}