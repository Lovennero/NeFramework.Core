using System.Threading;
using System.Threading.Tasks;

namespace Framework.Core.Sound
{
    public interface ISoundLifecycleLogic
    {
        ValueTask<SoundRecord> AcquireSound(string soundName, string groupName, string assetPath, ISoundParams soundParams, int priority = 0, CancellationToken ct = default);
        
        void Recycle(int id);
    }
}