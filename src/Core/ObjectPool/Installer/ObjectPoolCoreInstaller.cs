using Framework.Core.FrameDI;

namespace Framework.Core.ObjectPool
{
    public class ObjectPoolCoreInstaller : IInstaller
    {
        public void Install(IBluePrintHandle handle)
        {
            // === 运行数据 ===
            handle.Register<ObjectPoolModel>();

            handle.Register<ObjectPoolTickModel>();
            
            // === 运行逻辑 ===
            handle.Register<PoolLifecycleLogic>()
                .AddBind<IPoolLifecycleLogic>();

            handle.Register<PoolLogic>()
                .AddBind<IPoolLogic>();

            handle.Register<PoolQueryLogic>()
                .AddBind<IPoolQueryLogic>();

            handle.Register<PoolTickLogic>()
                .AddBind<IPoolTickLogic>();

            handle.Register<PoolTickSnapshotLogic>()
                .AddBind<IPoolTickSnapshotLogic>();
        }
    }
}