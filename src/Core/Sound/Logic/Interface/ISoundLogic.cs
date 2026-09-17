using System.Threading.Tasks;

namespace Framework.Core.Sound
{
    public interface ISoundLogic
    {
        ValueTask<bool> Play(int id, float fadeTime);
        ValueTask<bool> Stop(int id, float fadeTime);
        ValueTask<bool> Resume(int id, float fadeTime);
        ValueTask<bool> Pause(int id, float fadeTime);
        
        float GetVolume(int id);
        void SetVolume(int id, float volume);
        bool GetMute(int id);
        void SetMute(int id, bool mute);
        float GetPitch(int id);
        void SetPitch(int id, float pitch);

        bool Validity(int id);
        
        ISound GetRawSound(int id);
    }
}