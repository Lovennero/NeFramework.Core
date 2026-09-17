using System.Collections.Generic;

namespace Framework.Core.ObjectPool
{
    internal sealed class ObjectPoolModel
    {
        public readonly Dictionary<string, PoolRecord> Pools= new();
    }
}