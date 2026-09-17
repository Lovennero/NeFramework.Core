using System;

namespace Framework.Core.Sound
{
    internal sealed class SoundTickModel
    {
        public SoundGroupRecord[] Groups = Array.Empty<SoundGroupRecord>();
        public int GroupCount;

        public SoundRecord[][] GroupSounds = Array.Empty<SoundRecord[]>();
        public int[] GroupSoundCounts = Array.Empty<int>();
    }
}
