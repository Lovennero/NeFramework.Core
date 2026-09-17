using System.Threading;
using System.Threading.Tasks;

namespace Framework.Core.Entity
{
    public interface IEntityLifeLogic
    {
        ValueTask<EntityRecord> AcquireEntity(string entityName, string groupName, string assetPath, int priority = 0, CancellationToken ct = default);
        ValueTask RecycleEntity(int id);
    }
}