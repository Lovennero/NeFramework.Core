using System;
using System.Threading.Tasks;
using Framework.Core.FrameVariable;

namespace Framework.Core.Fsm
{
    internal sealed class FsmLogic : IFsmLogic
    {
        private const string Tag = "Fsm Logic";
        
        private readonly FsmModel _model;
        
        private readonly IFsmHelper _fsmHelper;
        
        private readonly FsmStateDestroyOperator _stateDestroyOp;
        
        public FsmLogic(FsmModel model, IFsmHelper fsmHelper, FsmStateDestroyOperator stateDestroyOp)
        {
            _model = model;
            
            _fsmHelper = fsmHelper;
            
            _stateDestroyOp = stateDestroyOp;
        }

        // === 周期逻辑 ===
        public FsmStateRecord AddState<T>(string fsmKey) where T : class, IState, new()
        {
            var fsmRecords = _model.FsmRecords;
            var fsmStateRecords = _model.FsmStateRecords;
            
            // 存在性检测
            if (!fsmRecords.TryGetValue(fsmKey, out var fsmRecord) || !fsmRecord.IsValid)
            {
                throw new InvalidOperationException($"[{Tag}]: Fsm: {fsmKey} does not exist.");
            }
            
            // 重复性检测
            var type = typeof(T);
            if (fsmRecord.States.ContainsKey(type))
            {
                throw new InvalidOperationException($"[{Tag}]: Fsm:{fsmKey} State:{type.Name} already exists.");
            }
            
            var serialID = _model.SerialID++;

            // 创建对象
            var state = new T();
            _fsmHelper.InjectState(fsmRecord.ScopeName, state);
            var stateRecord = new FsmStateRecord(serialID, fsmKey, type, state);
            
            fsmRecord.States.Add(type, serialID);
            fsmStateRecords.Add(serialID, stateRecord);

            stateRecord.State.OnInit();
            stateRecord.IsValid = true;
            
            return stateRecord;
        }

        public FsmStateRecord AddState<T>(string fsmKey, T state) where T : class, IState
        {
            var fsmRecords = _model.FsmRecords;
            var fsmStateRecords = _model.FsmStateRecords;
            
            // 存在性检测
            if (!fsmRecords.TryGetValue(fsmKey, out var fsmRecord) || !fsmRecord.IsValid)
            {
                throw new InvalidOperationException($"[{Tag}]: Fsm: {fsmKey} does not exist.");
            }
            
            // 重复性检测
            var type = typeof(T);
            if (fsmRecord.States.ContainsKey(type))
            {
                throw new InvalidOperationException($"[{Tag}]: Fsm:{fsmKey} State:{type.Name} already exists.");
            }
            
            var serialID = _model.SerialID++;

            // 创建对象
            _fsmHelper.InjectState(fsmRecord.ScopeName, state);
            var stateRecord = new FsmStateRecord(serialID, fsmKey, type, state);
            
            fsmRecord.States.Add(type, serialID);
            fsmStateRecords.Add(serialID, stateRecord);

            stateRecord.State.OnInit();
            stateRecord.IsValid = true;
            
            return stateRecord;
        }
        
        public async ValueTask RemoveState<T>(string fsmKey) where T : class, IState
        {
            var fsmRecords = _model.FsmRecords;
            var fsmStateRecords = _model.FsmStateRecords;
            
            // 存在性检测
            if (!fsmRecords.TryGetValue(fsmKey, out var record) || !record.IsValid)
            {
                throw new InvalidOperationException($"[{Tag}]: Fsm: {fsmKey} does not exist.");
            }
            // 重复性检测
            var type = typeof(T);
            if (!record.States.TryGetValue(type, out var serialID))
            {
                throw new InvalidOperationException($"[{Tag}]: Fsm:{fsmKey} State:{type.Name} does not exists.");
            }

            var stateRecord = fsmStateRecords[serialID];
            await _stateDestroyOp.Execute(stateRecord);
        }
        
        // === 改变逻辑 ===
        public async ValueTask StartState<T>(string fsmKey) where T : class, IState
        {
            var fsmRecords = _model.FsmRecords;
            var stateRecords = _model.FsmStateRecords;
            if (!fsmRecords.TryGetValue(fsmKey, out var fsmRecord))
            {
                throw new InvalidOperationException($"[{Tag}]: Fsm record {fsmKey} does not exist.");
            }
            
            var type =  typeof(T);
            if (!fsmRecord.States.TryGetValue(type, out var stateID))
            {
                throw new InvalidOperationException($"[{Tag}]: Fsm state:{type.Name} does not exist.");
            }

            if (!stateRecords.TryGetValue(stateID, out var stateRecord) || !stateRecord.IsValid)
            {
                throw new InvalidOperationException($"[{Tag}]: Fsm state data does not match");
            }
            
            fsmRecord.StateChangeTcs = new TaskCompletionSource<bool>();

            fsmRecord.CurrentState = stateID;
            fsmRecord.CurrentStateTime = 0f;
            await stateRecord.State.OnEnter();
            
            fsmRecord.StateChangeTcs.SetResult(true);
        }
        public async ValueTask ChangeState<T>(string fsmKey) where T : class, IState
        {
            var fsmRecords = _model.FsmRecords;
            var stateRecords = _model.FsmStateRecords;

            if (!fsmRecords.TryGetValue(fsmKey, out var fsmRecord))
            {
                throw new InvalidOperationException($"[{Tag}] Fsm record {fsmKey} does not exist.");
            }
            var type =  typeof(T);
            if (!fsmRecord.States.TryGetValue(type, out var stateID))
            {
                throw new InvalidOperationException($"[{Tag}]: Fsm state:{type.Name} does not exist.");
            }

            if (!stateRecords.TryGetValue(stateID, out var stateRecord) || !stateRecord.IsValid)
            {
                throw new InvalidOperationException($"[{Tag}]: Fsm state data does not match");
            }

            if (fsmRecord.CurrentState == -1)
            {
                fsmRecord.StateChangeTcs = new TaskCompletionSource<bool>();

                fsmRecord.CurrentState = stateID;
                fsmRecord.CurrentStateTime = 0f;
                await stateRecord.State.OnEnter();
                
                fsmRecord.StateChangeTcs.SetResult(true);
                return;
            }
            
            if(fsmRecord.CurrentState == stateID) return;
            
            fsmRecord.StateChangeTcs = new TaskCompletionSource<bool>();
            var currentStateRecord = stateRecords[fsmRecord.CurrentState];
            
            await currentStateRecord.State.OnExit();
            fsmRecord.CurrentState = stateID;
            fsmRecord.CurrentStateTime = 0f;
            await stateRecord.State.OnEnter();
            
            fsmRecord.StateChangeTcs.SetResult(true);
        }
        public async ValueTask StopState(string fsmKey)
        {
            var fsmRecords = _model.FsmRecords;
            var stateRecords = _model.FsmStateRecords;
            if (!fsmRecords.TryGetValue(fsmKey, out var fsmRecord))
            {
                throw new InvalidOperationException($"[{Tag}]: Fsm:{fsmKey} does not exist.");
            }

            var stateID = fsmRecord.CurrentState;
            if (stateID == -1)
            {
                throw new InvalidOperationException($"[{Tag}]: Fsm:{fsmKey} does not Start,can not Stop.");
            }
            
            if (!stateRecords.TryGetValue(stateID, out var stateRecord) || !stateRecord.IsValid)
            {
                throw new InvalidOperationException($"[{Tag}]: Fsm state data does not match");
            }
            
            // 执行停止逻辑
            fsmRecord.StateChangeTcs = new TaskCompletionSource<bool>();
            
            await stateRecord.State.OnExit();
            fsmRecord.CurrentState = -1;
            fsmRecord.CurrentStateTime = 0f;
            
            fsmRecord.StateChangeTcs.SetResult(true);
        }
        
        // === 状态查询 ===
        public bool Validity(string groupName)
        {
            var fsmRecords = _model.FsmRecords;
            if (!fsmRecords.TryGetValue(groupName, out var fsmRecord)) return false;
            return fsmRecord.IsValid;
        }
        public float CurrentStateTime(string groupName)
        {
            var fsmRecords = _model.FsmRecords;
            if (!fsmRecords.TryGetValue(groupName, out var fsmRecord))
            {
                throw new InvalidOperationException($"[{Tag}] Fsm record {groupName} does not exist.");
            }
            return fsmRecord.CurrentStateTime;
        }
        public FsmStateRecord CurrentStateType(string fsmKey)
        {
            var fsmRecords = _model.FsmRecords;
            var fsmStateRecords = _model.FsmStateRecords;
            if (!fsmRecords.TryGetValue(fsmKey, out var fsmRecord))
            {
                throw new InvalidOperationException($"[{Tag}] Fsm record {fsmKey} does not exist.");
            }
            
            var currentID = fsmRecord.CurrentState;
            if (!fsmStateRecords.TryGetValue(currentID, out var fsmStateRecord))
            {
                throw new InvalidOperationException($"[{Tag}] Fsm state data does not match");
            }
            
            return fsmStateRecord;
        }
        public bool HasState<T>(string fsmKey) where T : class, IState
        {
            var fsmRecords = _model.FsmRecords;
            if (!fsmRecords.TryGetValue(fsmKey, out var fsmRecord)) return false;
            return fsmRecord.States.ContainsKey(typeof(T));
        }
        public FsmStateRecord GetState<T>(string fsmKey) where T : class, IState
        {
            var fsmRecords = _model.FsmRecords;
            var fsmStateRecords = _model.FsmStateRecords;
            if (!fsmRecords.TryGetValue(fsmKey, out var fsmRecord))
            {
                throw new InvalidOperationException($"[{Tag}] Fsm record {fsmKey} does not exist.");
            }
            
            var type = typeof(T);
            if (!fsmRecord.States.TryGetValue(type, out var stateID))
            {
                throw new InvalidOperationException($"[{Tag}] Fsm state:{type.Name} data does not exist.");
            }

            return fsmStateRecords[stateID];
        }
        
        // === 上下文 ===
        public bool HasContext(string fsmKey, string contextKey)
        {
            var fsmRecords = _model.FsmRecords;
            if (!fsmRecords.TryGetValue(fsmKey, out var fsmRecord) || !fsmRecord.IsValid)
            {
                throw new InvalidOperationException($"[{Tag}]: Fsm record {fsmKey} does not exist.");
            }

            return fsmRecord.Context.ContainsKey(contextKey);
        }
        public FVariable<T> GetContext<T>(string fsmKey, string contextKey)
        {
            var fsmRecords = _model.FsmRecords;
            if (!fsmRecords.TryGetValue(fsmKey, out var fsmRecord) || !fsmRecord.IsValid)
            {
                throw new InvalidOperationException($"[{Tag}]: Fsm record {fsmKey} does not exist.");
            }

            var variable = fsmRecord.Context[contextKey];
            return variable as FVariable<T>;
        }
        public bool GetContext<T>(string fsmKey, string contextKey, out FVariable<T> fVariable)
        {
            var fsmRecords = _model.FsmRecords;
            if (!fsmRecords.TryGetValue(fsmKey, out var fsmRecord) || !fsmRecord.IsValid)
            {
                throw new InvalidOperationException($"[{Tag}]: Fsm record {fsmKey} does not exist.");
            }

            if (!fsmRecord.Context.TryGetValue(contextKey, out var value))
            {
                fVariable = null;
                return false;
            }

            fVariable = value as FVariable<T>;
            return true;
        }
        public void AddContext<T>(string fsmKey, string contextKey, FVariable<T> fVariable)
        {
            var fsmRecords = _model.FsmRecords;
            if (!fsmRecords.TryGetValue(fsmKey, out var fsmRecord) || !fsmRecord.IsValid)
            {
                throw new InvalidOperationException($"[{Tag}]: Fsm record {fsmKey} does not exist.");
            }
            
            fsmRecord.Context[contextKey] = fVariable;
        }
        public bool RemoveContext(string fsmKey, string contextKey)
        {
            var fsmRecords = _model.FsmRecords;
            if (!fsmRecords.TryGetValue(fsmKey, out var fsmRecord) || !fsmRecord.IsValid)
            {
                throw new InvalidOperationException($"[{Tag}]: Fsm record {fsmKey} does not exist.");
            }
            return fsmRecord.Context.Remove(contextKey);
        }
    }
}