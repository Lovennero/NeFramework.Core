using System.Collections.Generic;

namespace Framework.Core.Sound
{
    internal sealed class SoundModel
    {
        // === 映射数据 ===
        public readonly Dictionary<int, SoundRecord> Records = new();
        public readonly Dictionary<string, SoundGroupRecord> GroupRecords = new();

        // === 排序数据 ===
        public readonly List<SoundGroupRecord> GroupSort = new();
        public readonly Dictionary<string, List<SoundRecord>> GroupSoundSorts = new();

        // === 逻辑数据 ==
        public int SerialID = 0;
    }
}