using System.Collections.Generic;

namespace Framework.Core.Request
{
    public struct RequestConfig
    {
        // === 请求配置 ===
        public string URL;
        public RequestMethod RequestMethod;
        public Dictionary<string, string> Headers;
        public byte[] Body;
        
        // === 回应配置 ===
        public ResponseMethod ResponseMethod;
        public string SavePath;
        public bool Append;
    }
}