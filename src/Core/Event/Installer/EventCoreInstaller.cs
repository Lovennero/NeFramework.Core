using Framework.Core.FrameDI;

namespace Framework.Core.Event
{
    public class EventCoreInstaller : IInstaller
    {
        public void Install(IBluePrintHandle handle)
        {
            // === 核心数据 ===
            handle.Register<EventModel>();
            
            // === 核心逻辑 ===
            handle.Register<EventLifecycleLogic>()
                .AddBind<IEventLifecycleLogic>();
            
            handle.Register<EventLogic>()
                .AddBind<IEventLogic>();
        }
    }
}