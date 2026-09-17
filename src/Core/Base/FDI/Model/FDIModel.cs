using System;
using System.Collections.Generic;

namespace Framework.Core.FrameDI
{
    internal sealed class FDIModel
    {
        // === 工厂数据 ===
        public readonly Dictionary<Type, InstanceFactory> InstanceFactories = new ();
        public readonly Dictionary<Type, InjectFactory> InjectFactories = new ();
        
        // === 蓝图数据 ===
        public readonly Dictionary<string, BluePrintRecord> BluePrintRecords = new ();
        
        // === 作用域数据 ===
        public readonly Dictionary<string, ScopeRecord> ScopeRecords = new ();
        public readonly List<ScopeRecord> ScopeSort = new();
        public int SortVersion;
        
        // === 注册数据 ===
        public readonly Dictionary<int, RegisterRecord> RegisterRecords = new();
        public int RegisterID;
        
        // === 实例数据 ===
        public readonly Dictionary<int, object> TypeInstances = new ();
        public int InstanceID;
    }
}