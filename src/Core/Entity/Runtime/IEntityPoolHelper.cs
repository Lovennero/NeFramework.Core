namespace Framework.Core.Entity
{
    public interface IEntityPoolHelper
    {
        void InitPool(string poolName, IEntityAsset asset);
        public void ReleasePool(string assetName);
    }
}