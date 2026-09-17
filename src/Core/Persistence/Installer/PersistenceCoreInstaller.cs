using Framework.Core.FrameDI;

namespace Framework.Core.Persistence
{
    public class PersistenceCoreInstaller : IInstaller
    {
        public void Install(IBluePrintHandle builder)
        {
            // === 运行逻辑 ===
            builder.Register<PersistenceLogic>()
                .AddBind<IPersistenceLogic>();
        }
    }
}