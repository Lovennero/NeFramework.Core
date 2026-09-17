using System;
using System.Threading.Tasks;

namespace Framework.Core.Entity
{
    public interface IEntityProvider:IDisposable
    {
        public ValueTask<IEntityAsset> LoadAssetAsync(string assetPath);
        public void UnLoadAsset(string assetPath);
    }
}