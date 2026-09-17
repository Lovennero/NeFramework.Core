using System.Threading.Tasks;

namespace Framework.Core.UI
{
    public interface IUIGroupLifeHelper
    {
        ValueTask CreateGroupObj(string groupName, UILayer layer, IUIGroupParams groupParams);
        void DestroyGroupObj(string groupName);
    }
}