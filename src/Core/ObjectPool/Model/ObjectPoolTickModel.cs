using System;

namespace Framework.Core.ObjectPool
{
    public class ObjectPoolTickModel
    {
        public PoolRecord[] Pools = Array.Empty<PoolRecord>();
        public int PoolCount;
    }
}