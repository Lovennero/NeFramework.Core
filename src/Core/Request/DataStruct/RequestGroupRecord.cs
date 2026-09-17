using System;
using System.Collections.Generic;
using System.Threading;

namespace Framework.Core.Request
{
    public sealed class RequestGroupRecord : IComparable<RequestGroupRecord>
    {
        // === 基础参数 ===
        private int GroupSequence { get; }
        public string GroupKey { get; }
        private int Priority { get; }
        
        // === 配置参数 ===
        public RequestGroupConfig GroupConfig { get; }
        
        // === 运行参数 ===
        public HashSet<string> Members { get; }
        
        public int Concurrent { get; set; }
        
        internal RequestGroupRecord(int groupSequence, string groupKey, int priority, RequestGroupConfig config)
        {
            GroupSequence = groupSequence;
            GroupKey = groupKey;
            Priority = priority;
            
            GroupConfig = config;
            
            Members = new HashSet<string>();
        }

        public int CompareTo(RequestGroupRecord other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (other is null) return 1;

            var cmp = Priority.CompareTo(other.Priority);
            if (cmp == 0)
                cmp = GroupSequence.CompareTo(other.GroupSequence);
            return cmp;
        }
    }
}