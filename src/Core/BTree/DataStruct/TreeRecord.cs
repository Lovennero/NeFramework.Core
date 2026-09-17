using System.Collections.Generic;
using Framework.Core.FrameVariable;

namespace Framework.Core.BTree
{
    public sealed class BTreeRecord
    {
        // === 基础参数 ===
        public string Key { get; }
        
        // === 动态参数 ===
        public bool Valid { get; set; }
        public BTNode RootBtNode { get; set; }
        public Dictionary<string, IFVariable> Blackboard { get; set; }
        
        internal BTreeRecord(string key)
        {
            Key = key;
            Blackboard = new Dictionary<string, IFVariable>();
        }
    }
}