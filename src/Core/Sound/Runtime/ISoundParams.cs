namespace Framework.Core.Sound
{
    public interface ISoundParams
    {
        public bool Mute { get; set; }
        public float Volume{ get; set; }
        public float Pitch{ get; set; }
    }
}