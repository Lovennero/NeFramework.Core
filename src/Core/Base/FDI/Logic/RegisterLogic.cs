using System;

namespace Framework.Core.FrameDI
{
    internal sealed class RegisterLogic : IRegisterLogic
    {
        private const string Tag = "Register Logic";

        private readonly FDIModel _model;
        
        public RegisterLogic(FDIModel model)
        {
            _model = model;
        }

        // === 接口绑定逻辑 ===
        public void AddBind<T>(int rtID)
        {
            var bindType = typeof(T);
            if (!bindType.IsAbstract && !bindType.IsInterface)
            {
                throw new InvalidOperationException($"{bindType.FullName} is not an interface or abstract class.");
            }
            
            var rtRecords = _model.RegisterRecords;
            if (!rtRecords.TryGetValue(rtID, out var rtRecord) || !rtRecord.Valid)
            {
                throw new InvalidOperationException($"[{Tag}]: Register:{rtID} does not exist.");
            }

            var implType = rtRecord.ImpType;
            if (!bindType.IsAssignableFrom(implType))
            {
                throw new InvalidOperationException($"{bindType.FullName} is not assignable to {implType.FullName}");
            }
            rtRecord.BindTypes.Add(bindType);
        }
        public void RemoveBind<T>(int rtID)
        {
            var bindType = typeof(T);
            if (!bindType.IsAbstract && !bindType.IsInterface)
            {
                throw new InvalidOperationException($"{bindType.FullName} is not an interface or abstract class.");
            }
            
            var rtRecords = _model.RegisterRecords;
            if (!rtRecords.TryGetValue(rtID, out var rtRecord) || !rtRecord.Valid)
            {
                throw new InvalidOperationException($"[{Tag}]: Register:{rtID} does not exist.");
            }
            
            var implType = rtRecord.ImpType;
            if (!bindType.IsAssignableFrom(implType))
            {
                throw new InvalidOperationException($"{bindType.FullName} is not assignable to {implType.FullName}");
            }
            rtRecord.BindTypes.Remove(bindType);
        }
    }
}