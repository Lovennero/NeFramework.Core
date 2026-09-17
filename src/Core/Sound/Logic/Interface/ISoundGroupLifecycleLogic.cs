namespace Framework.Core.Sound
{
    public interface ISoundGroupLifecycleLogic
    {
        SoundGroupRecord AddGroup(string groupName, int priority, ISoundParams group);
        void RemoveGroup(string groupName);
    }
}