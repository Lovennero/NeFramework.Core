using System;
using System.Collections.Generic;

namespace Framework.Core.FrameDI
{
    public sealed class ScopeRecord
    {
        // === 基础参数 ===
        public string Name { get; }
        public IReadOnlyDictionary<Type, int> TypeRegisterMap { get; }
        public IReadOnlyDictionary<Type,List<int>> TypeBuildMap { get; } 
        
        // === 动态参数 ===
        public bool Valid { get; set; }
        public string Parent { get; set; }
        public HashSet<string> Children { get; }
        public Dictionary<int, int> RegisterInstanceMap { get; }
        
        // === 周期参数 ===
        public List<IStartable> Startable { get; }
        public List<ITickable> Tickable { get; }
        public List<ILateTickable> LateTickable { get; }
        public List<IReleasable> Releasable { get; }
        
        internal ScopeRecord(
            string name,
            IReadOnlyDictionary<Type, int> typeRegisterMap,
            IReadOnlyDictionary<Type,List<int>> typeBuildMap)
        {
            // 基本信息
            Name = name;
            TypeRegisterMap = typeRegisterMap;
            TypeBuildMap = typeBuildMap;
            
            // 动态数据
            Children = new HashSet<string>();
            RegisterInstanceMap = new Dictionary<int, int>();
            
            // 周期参数
            Startable = new List<IStartable>();
            Tickable = new List<ITickable>();
            LateTickable = new List<ILateTickable>();
            Releasable = new List<IReleasable>();
        }
    }
}