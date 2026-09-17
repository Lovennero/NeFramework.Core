using System.Threading;
using System.Threading.Tasks;

namespace Framework.Core.UI
{
    public interface IUILifeHelper
    {
        ValueTask<bool> Acquire(UIRecord record, string scopeName = "Frame", CancellationToken ct = default);
        void Recycle(UIRecord record);
    }
}