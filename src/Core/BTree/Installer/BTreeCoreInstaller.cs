using Framework.Core.FrameDI;

namespace Framework.Core.BTree
{
    public class BTreeCoreInstaller : IInstaller
    {
        public void Install(IBluePrintHandle handle)
        {
            // === 模块数据 ===
            handle.Register<BTreeModel>();
            
            // === 模块逻辑 ===
            handle.Register<BTreeBlackboardLogic>()
                .AddBind<IBTreeBlackboardLogic>();
            
            handle.Register<BTreeControlLogic>()
                .AddBind<IBTreeControlLogic>();
            
            handle.Register<BTreeLifeLogic>()
                .AddBind<IBTreeLifeLogic>();
        }
    }
}