using System.Collections.Generic;

namespace Framework.Core.Sound
{
    public class SoundGroupRecord
    {
        // === 基础信息 ===
        public string GroupName { get; }
        public int Priority { get; }
        public ISoundParams SoundParams { get; }
        
        // === 动态参数 ===
        public HashSet<int> Members { get; }

        public SoundGroupRecord(string groupName, int priority, ISoundParams soundParams)
        {
            GroupName = groupName;
            Priority = priority;
            SoundParams = soundParams;
            
            Members = new HashSet<int>();
        }
    }
}