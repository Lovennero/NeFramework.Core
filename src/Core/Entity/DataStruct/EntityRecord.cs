using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.Core.Entity
{
    public class EntityRecord
    {
        // === 基础参数 ===
        public int ID { get; }
        public string Name { get; }
        public string GroupName { get; }
        public string AssetPath { get; }
        public int Priority { get; }
        
        // === 动态参数 ===
        public ELifePhase LifePhase { get; set; }
        public TaskCompletionSource<bool> LifeTcs { get; set; }
        public EShowPhase ShowPhase { get; set; }
        public TaskCompletionSource<bool> ShowTcs { get; set; }

        // === 父子关系 ===
        public int ParentID { get; set; } = -1;
        public HashSet<int> ChildrenIDs { get; }

        // === 实体脚本 ===
        public IEntity Entity { get; set; }
        
        internal EntityRecord(int id, string name, string groupName, string assetPath, int priority)
        {
            ID = id;
            Name = name;
            GroupName = groupName;
            AssetPath = assetPath;
            Priority = priority;

            ChildrenIDs = new HashSet<int>();
        }
    }
}