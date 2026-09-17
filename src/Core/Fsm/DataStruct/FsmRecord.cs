using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework.Core.FrameVariable;

namespace Framework.Core.Fsm
{
    public class FsmRecord
    {
        // === 基础参数 ===
        public string FsmName { get; }
        public string ScopeName { get; }
        
        // === 动态参数 ===
        public int CurrentState { get; set; }
        public float CurrentStateTime { get; set; }
        public bool IsValid { get; set; }
        
        public TaskCompletionSource<bool> StateChangeTcs { get; set; }
        
        public Dictionary<Type, int> States { get; }
        public Dictionary<string, IFVariable> Context { get; set; }
        
        internal FsmRecord(string fsmName, string scopeName)
        {
            FsmName = fsmName;
            ScopeName = scopeName;

            CurrentState = -1;
            CurrentStateTime = 0f;
            IsValid = true;
            States = new Dictionary<Type, int>();
            Context = new Dictionary<string, IFVariable>();
        }
    }
}