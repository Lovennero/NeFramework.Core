using System;

namespace Framework.Core.Entity
{
    internal sealed class EntityTickModel
    {
        public EntityGroupRecord[] Groups = Array.Empty<EntityGroupRecord>();
        public int GroupCount;

        public EntityRecord[][] GroupEntities = Array.Empty<EntityRecord[]>();
        public int[] GroupEntityCounts = Array.Empty<int>();
        
        public EntityPoolRecord[] Pools = Array.Empty<EntityPoolRecord>();
        public int PoolCount;
    }
}