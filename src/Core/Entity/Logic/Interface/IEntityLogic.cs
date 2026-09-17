
using System.Threading.Tasks;

namespace Framework.Core.Entity
{
    public interface IEntityLogic
    {
        ELifePhase LifePhase(int id);
        EShowPhase ShowPhase(int id);
        ValueTask ShowEntity(int id);
        ValueTask HideEntity(int id);
        
        ValueTask MountLogicEntity(int parentId, int childId);
        IEntity GetEntity(int id);
    }
}