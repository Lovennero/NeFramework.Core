using System.Collections.Generic;

namespace Framework.Core.UI
{
    public sealed class UIGroupRecord
    {
        // === 基本参数 ===
        public string GroupName { get;}
        public int Priority { get; }
        public string ScopeName { get; }
        
        // === 动态参数 ===
        public bool IsValid { get; set; }
        public readonly HashSet<int> GroupMembers;
        public readonly List<int> Stack;
        
        internal UIGroupRecord(string groupName, int priority, string scopeName)
        {
            GroupName = groupName;
            Priority = priority;
            ScopeName = scopeName;
            
            GroupMembers = new HashSet<int>();
            Stack = new List<int>();
        }
    }
}