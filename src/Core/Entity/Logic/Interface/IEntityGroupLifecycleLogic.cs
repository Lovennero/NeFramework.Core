using System.Threading.Tasks;

namespace Framework.Core.Entity
{
    public interface IEntityGroupLifecycleLogic
    {
        EntityGroupRecord AddGroup(string groupName, int priority, string scopeName);
        ValueTask RemoveGroup(string groupName);
    }
}