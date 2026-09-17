using System;

namespace Framework.Core.FrameDI
{
    internal sealed class BluePrintLogic : IBluePrintLogic
    {
        private const string Tag = "Register Lifecycle Logic";

        private readonly FDIModel _model;
        
        public BluePrintLogic(FDIModel model)
        {
            _model = model;
        }
        
        // === 类型注册 ===
        public RegisterRecord Register<TConcrete>(string bluePrintName, ELifetime lifetime = ELifetime.Singleton) where TConcrete : class
        {
            var implType = typeof(TConcrete);

            var bpRecords = _model.BluePrintRecords;
            var rtRecords = _model.RegisterRecords;

            if (!bpRecords.TryGetValue(bluePrintName, out var bpRecord))
            {
                throw new InvalidOperationException($"[{Tag}]: Scope:{bluePrintName} does not exists.");
            }
            
            if (bpRecord.RtMap.TryGetValue(implType, out var implID))
            {
                throw new InvalidOperationException($"[{Tag}]: Implementation of type {implType.FullName} already exists.");
            }

            implID = _model.RegisterID ++;
            
            var cycle = GetTypeLifecycle(implType, lifetime);

            var record = new RegisterRecord(implID, bluePrintName, implType, lifetime, cycle, implType);
            
            bpRecord.RtMap.Add(implType, implID);
            rtRecords.Add(implID, record);
            record.Valid = true;
            
            return record;        
        }
        
        public void UnRegister<TConcrete>(string bluePrintName) where TConcrete : class
        {
            var implType = typeof(TConcrete);
            
            var bpRecords = _model.BluePrintRecords;
            var rtRecords = _model.RegisterRecords;

            if (!bpRecords.TryGetValue(bluePrintName, out var spRecord))
            {
                throw new InvalidOperationException($"[{Tag}]: Scope:{bluePrintName} does not exists.");
            }
            
            if (!spRecord.RtMap.TryGetValue(implType, out var implID))
            {
                throw new InvalidOperationException($"[{Tag}]: Implementation of type {implType.FullName} does not exists.");
            }
            
            var record = rtRecords[implID];
            record.Valid = false;
            spRecord.RtMap.Remove(implType);
            rtRecords.Remove(implID);
        }
        
        // === 周期注册 ===
        public void RegisterEntryPoint<TConcrete>(string name) where TConcrete : class
        {
            var implType = typeof(TConcrete);
            
            var bpRecords = _model.BluePrintRecords;
            if (!bpRecords.TryGetValue(name, out var bpRecord) || !bpRecord.Valid)
            {
                throw new InvalidOperationException($"[{Tag}]: Scope:{name} does not exists.");
            }
            
            var bpRtMap = bpRecord.RtMap;
            if (!bpRtMap.TryGetValue(implType, out var implID))
            {
                throw new InvalidOperationException($"[{Tag}]: Implementation of type {implType.FullName} does not exists.");
            }
            
            var rtRecords = _model.RegisterRecords;
            if (!rtRecords.TryGetValue(implID, out var rtRecord) || !rtRecord.Valid)
            {
                throw new  InvalidOperationException($"[{Tag}]: Register:{implType.FullName} does not exists.");
            }

            if (rtRecord.Lifetime == ELifetime.Transient)
            {
                throw new InvalidOperationException($"[{Tag}]: Register:{rtRecord.ImpType.FullName} is Transient, can not register Entry Point.");
            }
            
            rtRecord.EntryPoint = true;
        }

        public void UnregisterEntryPoint<TConcrete>(string name) where TConcrete : class
        {
            var implType = typeof(TConcrete);
            
            var bpRecords = _model.BluePrintRecords;
            if (!bpRecords.TryGetValue(name, out var bpRecord) || !bpRecord.Valid)
            {
                throw new InvalidOperationException($"[{Tag}]: Scope:{name} does not exists.");
            }
            
            var bpRtMap = bpRecord.RtMap;
            if (!bpRtMap.TryGetValue(implType, out var implID))
            {
                throw new InvalidOperationException($"[{Tag}]: Implementation of type {implType.FullName} does not exists.");
            }
            
            var rtRecords = _model.RegisterRecords;
            if (!rtRecords.TryGetValue(implID, out var rtRecord) || !rtRecord.Valid)
            {
                throw new  InvalidOperationException($"[{Tag}]: Register:{implType.FullName} does not exists.");
            }

            rtRecord.EntryPoint = false;
        }
        
        // === 内联方法 === 
        private ELifecycle GetTypeLifecycle(Type type, ELifetime lifetime)
        {
            var cycle = ELifecycle.None;
            
            if (lifetime == ELifetime.Transient) return cycle;
            
            var interfaces = type.GetInterfaces();
    
            foreach (var cycleInterface in interfaces)
            {
                if (cycleInterface == typeof(IStartable)) cycle |= ELifecycle.Startable;
                else if (cycleInterface == typeof(ITickable)) cycle |= ELifecycle.Tickable;
                else if (cycleInterface == typeof(ILateTickable)) cycle |= ELifecycle.LateTickable;
                else if (cycleInterface == typeof(IReleasable)) cycle |= ELifecycle.Releasable;
            }
    
            return cycle;
        }
    }
}