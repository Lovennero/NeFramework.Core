namespace Framework.Core.Sound
{
    public interface ISoundGroupParams
    {
        public float Volume { get; set; }
        public float Pitch { get; set; }
        public bool Mute { get; set; }
    }
}