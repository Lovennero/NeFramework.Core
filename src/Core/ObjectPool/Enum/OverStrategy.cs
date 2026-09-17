namespace Framework.Core.ObjectPool
{
    /// <summary>
    /// 池溢出策略
    /// </summary>
    public enum OverStrategy
    {
        /// <summary>
        /// 自动扩容
        /// </summary>
        AutoScale,
        
        /// <summary>
        /// 阻塞等待
        /// </summary>
        Block,
        
        /// <summary>
        /// 返空对象
        /// </summary>
        ReNull,
        
        /// <summary>
        /// 直接抛错
        /// </summary>
        Throw,
    }
}