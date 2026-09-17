using Framework.Core.FrameDI;

namespace Framework.Core.Sound
{
    public class SoundCoreInstaller:IInstaller
    {
        public int Order => 6;
        public void Install(IBluePrintHandle handle)
        {
            // === 核心数据 ===
            handle.Register<SoundModel>();
            
            handle.Register<SoundTickModel>();
            
            // === 原子操作 ===
            handle.Register<SoundGroupAddOperator>();
            
            handle.Register<SoundGroupRemoveOperator>();
            
            handle.Register<SoundRecycleOperator>();
            
            handle.Register<SoundAcquireOperator>();
            
            // === 依赖逻辑 ===
            handle.Register<SoundGroupLifecycleLogic>()
                .AddBind<ISoundGroupLifecycleLogic>();
            
            handle.Register<SoundLifecycleLogic>()
                .AddBind<ISoundLifecycleLogic>();
            
            // === 独立逻辑 ===
            handle.Register<SoundStartLogic>()
                .AddBind<ISoundStartLogic>();
            
            handle.Register<SoundGroupLogic>()
                .AddBind<ISoundGroupLogic>();
            
            handle.Register<SoundGeneralQueryLogic>()
                .AddBind<ISoundGeneralQueryLogic>();
            
            handle.Register<SoundLogic>()
                .AddBind<ISoundLogic>();
            
            handle.Register<SoundTickSnapshotLogic>()
                .AddBind<ISoundTickSnapshotLogic>();
            
            handle.Register<SoundTickLogic>()
                .AddBind<ISoundTickLogic>();
        }
    }
}