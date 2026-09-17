namespace Framework.Core.UI
{
    public sealed class UIRecord
    {
        // === 基本信息 ===
        public int ID { get; }
        public string Name { get; }
        public string GroupName { get; }
        public string AssetPath { get; }
        public int Priority { get; }
        
        // === 状态信息 ===
        public bool IsValid { get; set; }
        public bool IsShown { get; set; }
        
        // === 附属信息 ===
        public IUI UI { get; set; }
        
        internal UIRecord(int id, string name, string groupName, string assetPath, int priority)
        {
            ID = id;
            Name = name;
            GroupName = groupName;
            AssetPath = assetPath;
            Priority = priority;
        }
    }
}