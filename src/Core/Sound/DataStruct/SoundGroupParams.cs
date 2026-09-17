namespace Framework.Core.Sound
{
    public class SoundGroupParams : ISoundGroupParams
    {
        public float Volume { get; set; } = 1f;
        public float Pitch { get; set; } = 1f;
        public bool Mute { get; set; } = false;
    }
}