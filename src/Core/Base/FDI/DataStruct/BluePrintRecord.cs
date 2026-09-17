using System;
using System.Collections.Generic;

namespace Framework.Core.FrameDI
{
    public class BluePrintRecord
    {
        // === 基础数据 ===
        public string Name { get; }
        
        // === 动态数据 ===
        public bool Valid { get; set; }
        
        // === 注册数据 ===
        public Dictionary<Type, int> RtMap { get; }
        
        public BluePrintRecord(string name)
        {
            Name = name;

            RtMap = new Dictionary<Type, int>();
        }
    }
}