using System.Collections.Generic;

namespace Framework.Core.Fsm
{
    internal sealed class FsmModel
    {
        public readonly Dictionary<int,FsmStateRecord>  FsmStateRecords = new();
        public readonly Dictionary<string, FsmRecord> FsmRecords = new ();

        public int SerialID = 0;
    }
}