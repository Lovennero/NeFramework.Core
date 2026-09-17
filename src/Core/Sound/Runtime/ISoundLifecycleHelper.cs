using System.Threading;
using System.Threading.Tasks;

namespace Framework.Core.Sound
{
    public interface ISoundLifecycleHelper
    {
        ValueTask<bool> Acquire(SoundRecord record, CancellationToken ct = default);
        
        void Recycle(SoundRecord record);
    }
}