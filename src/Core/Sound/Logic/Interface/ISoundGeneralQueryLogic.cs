namespace Framework.Core.Sound
{
    public interface ISoundGeneralQueryLogic
    {
        public bool HasGroup(string groupName);
        public SoundGroupRecord GetGroup(string groupName);
        
        
        public bool HasSound(int id);
        public SoundRecord GetSound(int id);
    }
}