namespace Framework.Core.BTree
{
    public interface IBTreeControlLogic
    {
        // === 设置方法 ===
        void SetRootNode(string key, BTNode rootBtNode);
        
        // === 驱动方法 ===
        void Execute(string key);
        void Abort(string key);
    }
}