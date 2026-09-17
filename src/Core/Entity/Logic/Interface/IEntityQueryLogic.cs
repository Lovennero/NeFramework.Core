
namespace Framework.Core.Entity
{
    public interface IEntityQueryLogic
    {
        public bool HasGroup(string groupName);
        public EntityGroupRecord GetGroup(string groupName);
        
        public bool HasEntity(int id);
        public EntityRecord GetEntity(int id);
    }
}