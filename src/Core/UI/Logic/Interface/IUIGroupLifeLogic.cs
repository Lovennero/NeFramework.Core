using System.Threading.Tasks;

namespace Framework.Core.UI
{
    public interface IUIGroupLifeLogic
    {
        public ValueTask<UIGroupRecord> AddGroup(string groupName, int priority,  UILayer layer, IUIGroupParams groupParams, string scopeName = "Frame");
        public ValueTask RemoveGroup(string groupName);
    }
}