using System;

namespace Framework.Core.Asset
{
    public interface IAssetHandle
    {
        /// <summary>
        /// 资源加载进度.
        /// </summary>
        bool IsDone { get; }
        
        /// <summary>
        /// 资源加载进度.
        /// </summary>
        float Progress { get; }
    }
    
    public interface IAssetHandle<out T>:IDisposable where T:class
    {
        /// <summary>
        /// 资源对象.
        /// </summary>
        T Asset { get; }
    }
}
