using System.Threading;
using System.Threading.Tasks;

namespace Framework.Core.UI
{
    public interface IUILifeLogic
    {
        public ValueTask<UIRecord> AcquireUI(string groupName, string uiName, string assetPath, int priority = 0, CancellationToken ct = default);
        public ValueTask RecycleUI(int id);
    }
}