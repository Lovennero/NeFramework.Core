using System.Collections.Generic;

namespace Framework.Core.UI
{
    internal class UIModel
    {
        // === 映射数据 ===
        public readonly Dictionary<int, UIRecord> Records = new();
        public readonly Dictionary<string, UIGroupRecord> Groups = new();
        public readonly Dictionary<string, UIPoolRecord> Pools = new();
        
        // === 排序数据 ===
        public readonly List<UIGroupRecord> GroupsSort = new();
        public readonly Dictionary<string, List<UIRecord>> GroupsUIsSort = new();
        public readonly List<UIPoolRecord> PoolsSort = new();
        
        // === 逻辑数据 ===
        public int SerialID = 0;
    }
}