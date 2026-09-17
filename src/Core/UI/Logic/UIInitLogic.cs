namespace Framework.Core.UI
{
    public class UIInitLogic : IUIInitLogic
    {
        private readonly IUIInitHelper _helper;
        
        public UIInitLogic(IUIInitHelper helper)
        {
            _helper = helper;
        }

        public void InitUIRoot()
        {
            _helper.InitUIRoot();
        }
    }
}