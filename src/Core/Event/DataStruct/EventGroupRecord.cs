using System;
using System.Collections.Generic;

namespace Framework.Core.Event
{
    public sealed class EventGroupRecord
    {
        // === 基础参数 ===
        public string Key { get; }
        public Type Type { get; }
        
        // === 动态参数 ===
        public bool Validity { get; set; }
        public List<int> Members { get; }
        
        internal EventGroupRecord(string key, Type type)
        {
            // 基础参数
            Key = key;
            Type = type;
            
            // 动态参数
            Members = new List<int>();
        }
    }
}