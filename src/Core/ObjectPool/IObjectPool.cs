using System.Threading;
using System.Threading.Tasks;

namespace Framework.Core.ObjectPool
{
    public interface IObjectPool
    {
        // === 基本信息 ===
        public string Name { get; }
        public OverStrategy Strategy { get; }
        
        // === 容量相关 ===
        public int TotalCount { get; }
        public int ActiveCount { get; }
        public int CacheCount { get; }
        public int Capacity { get; }
        
        // === 清理相关 ===
        public float AutoReleaseInterval { get; }
        public float ExpirationTime { get; }
        
        // === 阻塞专属 ===
        public int BlockTimeout { get; set; }
        
        
        void OnUpdate(float logicTime, float realTime );
        void Release();
    }
    
    public interface IObjectPool<T> : IObjectPool where T : class, IObject
    {
        ValueTask<T> SpawnAsync(CancellationToken ct = default);
        void UnSpawn(T poolable);
    }
}