using System.Threading.Tasks;

namespace Framework.Core.Sound
{
    public interface ISound
    {
        // === 周期状态 ===
        bool AutoStop { get; }
        
        // === 生命周期 ===
        public void OnAcquire(ISoundAsset soundAsset, ISoundParams soundParams, ISoundParams groupParams);
        public void OnUpdate(float logicTime,float realTime);
        public void OnRecycle();
        
        // === 属性设置 ===
        public void RefreshParams();
        
        // === 流程控制 ===
        public ValueTask<bool> Play(float fadeTime = 0F);
        public ValueTask<bool> Stop(float fadeTime = 0F);
        public ValueTask<bool> Resume(float fadeTime = 0F);
        public ValueTask<bool> Pause(float fadeTime = 0F);
    }
}