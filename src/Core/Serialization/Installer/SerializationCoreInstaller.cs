
using Framework.Core.FrameDI;

namespace Framework.Core.Serialization
{
    public class SerializationCoreInstaller : IInstaller
    {
        public void Install(IBluePrintHandle builder)
        {
            // === 运行数据 ===
            builder.Register<SerModel>();
            
            // === 运行逻辑 ===
            builder.Register<SerConfigLifecycleLogic>()
                .AddBind<ISerConfigLifecycleLogic>();
            
            builder.Register<SerLogic>()
                .AddBind<ISerLogic>();
        }
    }
}