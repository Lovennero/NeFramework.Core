using System.Collections.Generic;

namespace Framework.Core.Entity
{
    public interface IEntityGroupLogic
    {
        public List<EntityRecord> GetGroupEntities(string groupName);
    }
}