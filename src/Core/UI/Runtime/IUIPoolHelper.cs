namespace Framework.Core.UI
{
    public interface IUIPoolHelper
    {
        void InitPool(string poolName, IUIAsset asset);
        public void ReleasePool(string assetName);
    }
}