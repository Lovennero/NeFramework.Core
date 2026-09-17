using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.Core.Sound
{
    public interface ISoundGroupLogic
    {
        float GetGroupVolume(string groupName);
        void SetGroupVolume(string groupName, float volume);
        bool GetGroupMute(string groupName);
        void SetGroupMute(string groupName, bool mute);
        float GetGroupPitch(string groupName);
        void SetGroupPitch(string groupName, float pitch);
        
        List<SoundRecord> GetGroupRecords(string groupName);
        ValueTask StopGroupAllSound(string groupName, float fadeInTime);
    }
}