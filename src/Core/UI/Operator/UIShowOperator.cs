using System.Threading.Tasks;

namespace Framework.Core.UI
{
    internal sealed class UIShowOperator
    {
        private readonly IUIHelper _helper;
        
        public UIShowOperator(IUIHelper helper)
        {
            _helper = helper;
        }

        public async ValueTask Execute(UIRecord record)
        {
            if(record.IsShown) return;
            
            _helper.ShowUI(record.ID);
            await record.UI.OnShow();
            record.IsShown = true;
        }
    }
}