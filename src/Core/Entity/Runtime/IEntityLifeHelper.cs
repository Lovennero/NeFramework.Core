using System.Threading;
using System.Threading.Tasks;

namespace Framework.Core.Entity
{
    public interface IEntityLifeHelper
    {
        ValueTask<bool> Acquire(EntityRecord record, string scopeName, CancellationToken ct = default);
        void Recycle(EntityRecord record);
    }
}