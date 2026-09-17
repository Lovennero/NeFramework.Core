using System;
using System.Threading.Tasks;

namespace Framework.Core.Fsm
{
    public sealed class FsmStateRecord
    {
        // === 基本参数 ===
        public int SerialID { get; }
        public string FsmKey { get; }
        public Type StateType { get; }
        public IState State { get; }
        
        // === 运行参数 ===
        public bool IsValid { get; set; }
        
        internal FsmStateRecord(
            int serialID,
            string fsmKey,
            Type stateType, 
            IState state)
        {
            SerialID = serialID;
            FsmKey = fsmKey;
            StateType = stateType;
            State = state;
        }
    }
}