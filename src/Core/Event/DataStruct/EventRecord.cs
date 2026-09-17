using System;

namespace Framework.Core.Event
{
    public sealed class EventRecord
    {
        // === 基本信息 ===
        public int ID { get; }
        public Type Type { get; }
        public string EventKey { get; }
        public Delegate Handler { get; }
        
        // === 动态参数 ===
        public bool Validity { get; set; }
        
        internal EventRecord(int id, Type type, string eventKey, Delegate handler)
        {
            ID = id;
            Type = type;
            EventKey = eventKey;
            Handler = handler;
        }
    }
}