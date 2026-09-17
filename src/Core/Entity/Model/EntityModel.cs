using System.Collections.Generic;

namespace Framework.Core.Entity
{
    internal sealed class EntityModel
    {
        // === 映射数据 ===
        public readonly Dictionary<int, EntityRecord> Records =  new();
        public readonly Dictionary<string, EntityGroupRecord>  Groups = new();
        public readonly Dictionary<string, EntityPoolRecord> Pools = new();
        
        // === 排序数据 ===
        public readonly List<EntityGroupRecord> GroupSort = new();
        public readonly Dictionary<string, List<EntityRecord>> GroupEntitySorts = new();
        public readonly List<EntityPoolRecord> PoolSorts = new();
        
        // === 逻辑数据 ===
        public int SerialID = 0;
    }
}