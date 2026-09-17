using System.Threading.Tasks;

namespace Framework.Core.UI
{
    public interface IUILogic
    {
        public bool Validity(int id);
        public bool Visibility(int id);
        
        public ValueTask ShowUI(int id);
        public ValueTask HideUI(int id);
        
        public IUI GetUI(int id);
    }
}