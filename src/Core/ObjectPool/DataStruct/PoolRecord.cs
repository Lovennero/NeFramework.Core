using System;

namespace Framework.Core.ObjectPool
{
    public sealed class PoolRecord
    {
        // === 基本参数 ===
        public string Name { get; }
        public OverStrategy Strategy { get; }
        
        // === 动态参数 ===
        public bool IsValid { get; set; }
        
        // === 对象参数 ===
        public IObjectPool Pool { get; }
        
        internal PoolRecord(string name, OverStrategy strategy, IObjectPool pool)
        {
            Name = name;
            Strategy = strategy;
            Pool = pool;
            IsValid = true;
        }
    }
}