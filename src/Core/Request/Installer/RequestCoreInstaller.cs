
using Framework.Core.FrameDI;

namespace Framework.Core.Request
{
    public class RequestCoreInstaller : IInstaller
    {
        public int Order => 7;
        
        public void Install(IBluePrintHandle builder)
        {
            // === 注册数据 ===
            builder.Register<RequestModel>();
            
            builder.Register<RequestTickModel>();
            
            // === 原子操作 ===
            builder.Register<RequestCancelOperator>();
            
            // === 运行逻辑 ===
            builder.Register<RequestGroupLifecycleLogic>()
                .AddBind<IRequestGroupLifecycleLogic>();
            
            builder.Register<RequestLifecycleLogic>()
                .AddBind<IRequestLifecycleLogic>();
            
            builder.Register<RequestLogic>()
                .AddBind<IRequestLogic>();
            
            builder.Register<RequestTickLogic>()
                .AddBind<IRequestTickLogic>();
        }
    }
}