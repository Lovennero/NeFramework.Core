namespace Framework.Core.Entity
{
    public interface IEntityGroupHelper
    {
        void InitGroup(string groupName, IEntityAsset asset ,int capacity, float autoReleaseInterval, float expirationTime);
        
        void RemoveGroup(string groupName);
    }
}