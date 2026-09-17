using Framework.Core.FrameDI;

namespace Framework.Core.Network
{
    public class NetCoreInstaller : IInstaller
    {
        public void Install(IBluePrintHandle builder)
        {
            // === 注册数据 ===
            builder.Register<NetModel>();
            
            builder.Register<NetTickModel>();
            
            // === 运行逻辑 ===
            builder.Register<NetLifecycleLogic>()
                .AddBind<INetLifecycleLogic>();
            
            builder.Register<NetLogic>()
                .AddBind<INetLogic>();

            builder.Register<NetQueryLogic>()
                .AddBind<INetQueryLogic>();
            
            builder.Register<NetTickLogic>()
                .AddBind<INetTickLogic>();
        }
    }
}