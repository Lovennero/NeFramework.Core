using System.Collections.Generic;

namespace Framework.Core.Request
{
    internal sealed class RequestModel
    {
        // === 映射数据 ===
        public readonly Dictionary<string, RequestRecord> Records = new();
        public readonly Dictionary<string, RequestGroupRecord> Groups = new();
        
        // === 运行数据 ===
        public readonly SortedSet<RequestGroupRecord> GroupsSort = new();
        public readonly Dictionary<string, SortedSet<RequestRecord>> GroupsWaiters = new();
        public readonly Dictionary<string, List<RequestRecord>> GroupsRunners = new();
        public readonly Dictionary<string, HashSet<RequestRecord>> GroupsRetriers = new();
        public int GroupSequence = 0;
        public int Sequence = 0;
    }
}