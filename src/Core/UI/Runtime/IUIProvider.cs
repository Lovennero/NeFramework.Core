using System.Threading.Tasks;

namespace Framework.Core.UI
{
    public interface IUIProvider
    {
        public ValueTask<IUIAsset> LoadAssetAsync(string assetPath);
        public void UnLoadAsset(string assetPath);
    }
}