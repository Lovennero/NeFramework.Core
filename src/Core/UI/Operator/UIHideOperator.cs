using System.Threading.Tasks;

namespace Framework.Core.UI
{
    internal class UIHideOperator
    {
        private readonly IUIHelper _helper;
        
        public UIHideOperator(IUIHelper helper)
        {
            _helper = helper;
        }

        public async ValueTask Execute(UIRecord record)
        {
            if(!record.IsShown) return;
            
            await record.UI.OnHide();
            _helper.HideUI(record.ID);
            record.IsShown = false;
        }
    }
}