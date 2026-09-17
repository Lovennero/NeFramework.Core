using System.Threading;
using System.Threading.Tasks;

namespace Framework.Core.ObjectPool
{
    public interface IPoolLogic
    {
        ValueTask<T> SpawnAsync<T>(string poolName, CancellationToken ct = default) where T : class, IObject;
        void UnSpawn<T>(string poolName, T obj) where T : class, IObject;
    }
}