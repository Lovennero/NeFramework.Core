using System;
using System.Collections.Generic;

namespace Framework.Core.FrameDI
{
    public class RegisterRecord
    {
        // === 基本参数 ===
        public int SerialID { get; }
        public string Name { get; }
        public Type ImpType { get; }
        public ELifetime Lifetime { get; }
        public ELifecycle Lifecycle { get; }
        
        // === 运行参数 ===
        public bool Valid { get; set; }
        public bool EntryPoint { get; set; }
        public HashSet<Type> BindTypes { get; }

        public RegisterRecord(
            int serialID,
            string name,
            Type impType,
            ELifetime lifetime,
            ELifecycle lifeCycle,
            params Type[] initBinds)
        {
            SerialID = serialID;
            Name = name;
            ImpType = impType;

            Lifetime = lifetime;
            Lifecycle = lifeCycle;
            
            BindTypes = new HashSet<Type>(initBinds);
        }
    }
}