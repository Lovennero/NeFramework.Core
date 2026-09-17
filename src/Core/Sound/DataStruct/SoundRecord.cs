namespace Framework.Core.Sound
{
    public class SoundRecord
    {
        // === 基本信息 ===
        public int ID { get; }
        public string Name { get; }
        public string AssetPath { get; }
        public int Priority { get; }
        public string GroupName { get; }
        public ISound Sound { get; set; }
        
        // === 状态参数 ===
        public bool IsValid { get; set; }
        
        // === 参数信息 ===
        public ISoundParams SoundParams { get; }
        
        public SoundRecord(int id, string name, string assetPath, int priority, string groupName, ISoundParams soundParams)
        {
            ID = id;
            Name = name;
            AssetPath = assetPath;
            Priority = priority;
            GroupName = groupName;
            SoundParams = soundParams;
        }
    }
}