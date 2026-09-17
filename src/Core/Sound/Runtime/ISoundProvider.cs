using System.Threading;
using System.Threading.Tasks;

namespace Framework.Core.Sound
{
    public interface ISoundProvider
    {
        public ISoundAsset LoadSound(string soundPath);
        
        public ValueTask<ISoundAsset> LoadSoundAsync(string soundPath,CancellationToken ct = default);
        
        public void UnloadSound(string soundPath);
    }
}