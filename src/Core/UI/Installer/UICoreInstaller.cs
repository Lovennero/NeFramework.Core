using Framework.Core.FrameDI;

namespace Framework.Core.UI
{
    public class UICoreInstaller:IInstaller
    {
        public int Order => 5;
        public void Install(IBluePrintHandle handle)
        {
            // === 核心数据 ===
            handle.Register<UIModel>();

            handle.Register<UITickModel>();
            
            // === 原子操作 ===
            handle.Register<UIAcquireOperator>();

            handle.Register<UIRecycleOperator>();

            handle.Register<UIShowOperator>();

            handle.Register<UIHideOperator>();

            handle.Register<UIPoolReleaseOperator>();
            
            // === 核心逻辑 ===
            handle.Register<UIInitLogic>()
                .AddBind<IUIInitLogic>();

            handle.Register<UIGroupLifeLogic>()
                .AddBind<IUIGroupLifeLogic>();

            handle.Register<UIGroupLogic>()
                .AddBind<IUIGroupLogic>();

            handle.Register<UILifeLogic>()
                .AddBind<IUILifeLogic>();

            handle.Register<UILogic>()
                .AddBind<IUILogic>();

            handle.Register<UIQueryLogic>()
                .AddBind<IUIQueryLogic>();

            handle.Register<UITickSnapshotLogic>()
                .AddBind<IUITickSnapshotLogic>();

            handle.Register<UITickLogic>()
                .AddBind<IUITickLogic>();
        }
    }
}