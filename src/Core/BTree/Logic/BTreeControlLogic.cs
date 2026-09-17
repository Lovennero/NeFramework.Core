using System;

namespace Framework.Core.BTree
{
    internal sealed class BTreeControlLogic : IBTreeControlLogic
    {
        private const string Tag = "BTree Control Logic";

        private BTreeModel BTreeModel { get; }
        
        public BTreeControlLogic(BTreeModel bTreeModel)
        {
            BTreeModel = bTreeModel;
        }
        
        // === 设置方法 ===
        public void SetRootNode(string key, BTNode rootBtNode)
        {
            var records = BTreeModel.BTreeRecords;
            if (!records.TryGetValue(key, out var record))
            {
                throw new InvalidOperationException($"[{Tag}]: BTree:{key} does not exist.");
            }

            if (record.RootBtNode != null)
            {
                // 中断当前根任务
                record.RootBtNode.Abort();
                record.RootBtNode = null;
                
                // 清楚缓存根黑板
                record.Blackboard.Clear();
            }
            
            // 设置根节点
            record.RootBtNode = rootBtNode;
        }
        
        // === 控制方法 ===
        public void Execute(string key)
        {
            var records = BTreeModel.BTreeRecords;
            if (!records.TryGetValue(key, out var record) || !record.Valid)
            {
                throw new InvalidOperationException($"[{Tag}]: BTree:{key} does not exist.");
            }

            if (record.RootBtNode == null)
            {
                throw new InvalidOperationException($"[{Tag}]: BTree:{key} root node is null, please set it first.");
            }
            
            record.RootBtNode.Execute();
        }
        public void Abort(string key)
        {
            var records = BTreeModel.BTreeRecords;
            if (!records.TryGetValue(key, out var record) ||  !record.Valid)
            {
                throw new InvalidOperationException($"[{Tag}]: BTree:{key} does not exist.");
            }
            
            if (record.RootBtNode == null)
            {
                throw new InvalidOperationException($"[{Tag}]: BTree:{key} root node is null, please set it first.");
            }
            
            record.RootBtNode.Abort();
        }
    }
}