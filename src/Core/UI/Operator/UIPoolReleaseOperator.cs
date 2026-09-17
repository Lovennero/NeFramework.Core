namespace Framework.Core.UI
{
    internal class UIPoolReleaseOperator
    {
        private readonly UIModel _model;
        private readonly IUIPoolHelper _helper;
        private readonly IUIProvider _provider;
        
        public UIPoolReleaseOperator(UIModel model,IUIPoolHelper uiPoolHelper,IUIProvider provider)
        {
            _model = model;
            _helper = uiPoolHelper;
            _provider = provider;
        }

        public void Execute(UIPoolRecord record)
        {
            _helper.ReleasePool(record.PoolName);
            record.IsValid = false;
            
            _model.Pools.Remove(record.PoolName);
            _model.PoolsSort.Remove(record);
            
            _provider.UnLoadAsset(record.PoolName);
        }
    }
}