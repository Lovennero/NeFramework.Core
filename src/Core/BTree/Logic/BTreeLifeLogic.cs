using System;

namespace Framework.Core.BTree
{
    internal sealed class BTreeLifeLogic : IBTreeLifeLogic
    {
        private const string Tag = "BTree Life Logic";

        private BTreeModel BTreeModel { get; }

        public BTreeLifeLogic(BTreeModel bTreeModel)
        {
            BTreeModel = bTreeModel;
        }

        public BTreeRecord BTreeCreate(string key)
        {
            var records = BTreeModel.BTreeRecords;
            if (records.TryGetValue(key, out var record))
            { 
                throw new InvalidOperationException($"[{Tag}]: BTree:{key} has already existed.");
            }
            
            record = new BTreeRecord(key);
            
            records.Add(key, record);
            record.Valid = true;
            
            return record;
        }

        public void BTreeRelease(string key)
        {
            var records = BTreeModel.BTreeRecords;
            if (!records.TryGetValue(key, out var record) || !record.Valid)
            {
                throw new InvalidOperationException($"[{Tag}]: BTree:{key} does not exist.");
            }
            
            record.RootBtNode?.Abort();
            
            record.Valid = false;
            records.Remove(key);
        }
    }
}