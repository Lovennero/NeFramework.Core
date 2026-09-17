using System;

namespace Framework.Core.Request
{
    public sealed class RequestRecord:IComparable<RequestRecord>
    {
        // === 基础信息 ===
        public string RequestKey { get; }
        public string GroupKey { get; }
        private int Sequence { get; }
        private int Priority { get; }
        
        // === 配置信息 ===
        public RequestConfig Config { get; }
        
        // === 运行信息 ===
        public IRequest Request { get; }
        
        internal RequestRecord(string requestKey, string groupKey, int priority ,int sequence ,RequestConfig config ,IRequest request)
        {
            // === 基础信息 ===
            Sequence = sequence;
            RequestKey = requestKey;
            GroupKey = groupKey;
            Priority = priority;
            
            // === 配置信息 ===
            Config = config;
            
            // === 运行信息 ===
            Request = request;
        }

        public int CompareTo(RequestRecord other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (other is null) return 1;

            var cmp = Priority.CompareTo(other.Priority);
            if (cmp == 0)
                cmp = Sequence.CompareTo(other.Sequence);
            return cmp;
        }
    }
}