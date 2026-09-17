using System.Collections.Generic;

namespace Framework.Core.Entity
{
    public sealed class EntityGroupRecord
    {
        // === 基础参数 ===
        public string GroupName { get; }
        public int Priority { get; }
        public string ScopeName { get; }
        
        // === 动态参数 ===
        public bool Valid { get; set; }
        public HashSet<int> Members { get; }
        
        internal EntityGroupRecord(string groupName, int priority, string scopeName)
        {
            GroupName = groupName;
            Priority = priority;
            ScopeName = scopeName;
            
            Members = new HashSet<int>();
        }
    }
}