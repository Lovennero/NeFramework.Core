using Framework.Core.FrameDI;

namespace Framework.Core.Entity
{
    public class EntityCoreInstaller : IInstaller
    {
        public void Install(IBluePrintHandle handle)
        {
            // === 核心数据 ===
            handle.Register<EntityModel>();
            
            handle.Register<EntityTickModel>();
            
            // === 原子操作 ===
            handle.Register<EntityRecycleOperator>();
            
            handle.Register<EntityPoolReleaseOperator>();
            
            // === 依赖逻辑 ===
            handle.Register<EntityStartLogic>()
                .AddBind<IEntityStartLogic>();
                
            handle.Register<EntityLifeLogic>()
                .AddBind<IEntityLifeLogic>();
            
            handle.Register<EntityGroupLifecycleLogic>()
                .AddBind<IEntityGroupLifecycleLogic>();
            
            handle.Register<EntityLogic>()
                .AddBind<IEntityLogic>();
            
            handle.Register<EntityGroupLogic>()
                .AddBind<IEntityGroupLogic>();
            
            // === 独立逻辑 ===
            handle.Register<EntityQueryLogic>()
                .AddBind<IEntityQueryLogic>();
            
            handle.Register<EntityTickLogic>()
                .AddBind<IEntityTickLogic>();
            
            handle.Register<EntityTickSnapshotLogic>()
                .AddBind<IEntityTickSnapshotLogic>();
        }
    }
}