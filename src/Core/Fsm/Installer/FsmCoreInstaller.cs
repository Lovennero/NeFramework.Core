using Framework.Core.FrameDI;

namespace Framework.Core.Fsm
{
    public class FsmCoreInstaller:IInstaller
    {
        public void Install(IBluePrintHandle builder)
        {
            // === 运行数据 ===
            builder.Register<FsmModel>();

            builder.Register<FsmTickModel>();
            
            // === 原子操作 ===
            builder.Register<FsmStateDestroyOperator>();
            
            // === 运行逻辑 ===
            builder.Register<FsmLifeLogic>()
                .AddBind<IFsmLifeLogic>();

            builder.Register<FsmLogic>()
                .AddBind<IFsmLogic>();

            builder.Register<FsmQueryLogic>()
                .AddBind<IFsmQueryLogic>();

            builder.Register<FsmTickLogic>()
                .AddBind<IFsmTickLogic>();

            builder.Register<FsmTickSnapshotLogic>()
                .AddBind<IFsmTickSnapshotLogic>();
        }
    }
}