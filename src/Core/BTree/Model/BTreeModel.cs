using System.Collections.Generic;

namespace Framework.Core.BTree
{
    internal sealed class BTreeModel
    {
        public readonly Dictionary<string, BTreeRecord> BTreeRecords = new();
    }
}