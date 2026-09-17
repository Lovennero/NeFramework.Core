using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Core.FrameLog;


namespace Framework.Core.FrameDI
{
    internal sealed class ScopeLifeLogic : IScopeLifeLogic
    {
        private const string Tag = "Scope Lifecycle Logic";
        
        private readonly FDIModel _model;
        
        private readonly ScopeResolveOperator _spResolveOp;
        
        public ScopeLifeLogic(FDIModel model, ScopeResolveOperator spResolveOp)
        {
            _model = model;
            _spResolveOp = spResolveOp;
        }

        public ScopeRecord CreateScope(string name, string parentName)
        {
            var bpRecords = _model.BluePrintRecords;
            var rtRecords = _model.RegisterRecords;
            var spRecords = _model.ScopeRecords;
            
            if (!bpRecords.TryGetValue(name, out var bpRecord))
            {
                throw new InvalidOperationException($"[{Tag}]: Scope:{name} does not exists.");
            }
            
            if (spRecords.TryGetValue(name, out var spRecord))
            {
                throw new InvalidOperationException($"[{Tag}]: Scope:{name} already exists.");
            }
            
            // 父子关系设置
            ScopeDependencyCheck(parentName);
            
            // 构建周期表、绑定表
            var entryPoints = new List<RegisterRecord>();
            var rtMap = bpRecord.RtMap;
            
            var typeMap = new Dictionary<Type, List<int>>();
            foreach (var rtID in rtMap.Values)
            {
                var rtRecord = rtRecords[rtID];
                
                // 标记周期对象
                if(rtRecord.EntryPoint) entryPoints.Add(rtRecord);
                
                // 构建绑定隐射
                foreach (var bindType in rtRecord.BindTypes)
                {
                    if (!typeMap.TryGetValue(bindType, out var ids))
                    {
                        ids = new List<int>();
                        typeMap[bindType] = ids;
                    }
                    ids.Add(rtID);
                }
            }
            
            // 创建作用域
            var tmpRtMap = new Dictionary<Type, int>(rtMap);
            spRecord = new ScopeRecord(name, tmpRtMap, typeMap);
            spRecords[name] = spRecord;
            
            // 建立父子关系
            ScopeRelationshipBuild(spRecord, parentName);
            
            // 标记有效
            spRecord.Valid = true;
            
            // 移除构建蓝图
            bpRecord.Valid = false;
            bpRecords.Remove(bpRecord.Name);
            bpRecord.RtMap.Clear();
            
            // 构建生命周期
            EntryPointBuild(spRecord, entryPoints);
            
            // 触发开始周期
            StartableTrigger(spRecord);
            
            return spRecord;
        }
        public void ReleaseScope(string scopeName)
        {
            var spRecords = _model.ScopeRecords;
            var spSort = _model.ScopeSort;
            var rtRecords = _model.RegisterRecords;
            var itRecords =  _model.TypeInstances;
            
            if (!spRecords.TryGetValue(scopeName, out var spRecord) || !spRecord.Valid)
            {
                throw new InvalidOperationException($"[{Tag}]: Scope:{scopeName} does not exists.");
            }
            
            // 递归父子关系
            var children = spRecord.Children.ToArray();
            foreach (var child in children)
            {
                ReleaseScope(child);
            }
            
            // 触发释放周期
            ReleasableTrigger(spRecord);
            
            // 移除周期缓存
            spRecord.Startable.Clear();
            spRecord.Tickable.Clear();
            spRecord.LateTickable.Clear();
            spRecord.Releasable.Clear();
            
            spRecord.Valid = false;
            
            // 解除父子关系
            if (spRecord.Parent != null)
            {
                var spParentRecord = spRecords[spRecord.Parent];
                spParentRecord.Children.Remove(spRecord.Name);
                spRecord.Parent = null;
            }
            
            // 移除内部记录
            spRecords.Remove(spRecord.Name);
            
            // 移除排序逻辑
            spSort.Remove(spRecord);
            _model.SortVersion++;
            
            // 移除缓存实例
            var itMap = spRecord.RegisterInstanceMap;
            foreach (var itID in itMap.Values)
            {
                itRecords.Remove(itID);
            }
            
            // 移除注册关系
            var rtMap = spRecord.TypeRegisterMap;
            foreach (var rtID in rtMap.Values)
            {
                var rtRecord = rtRecords[rtID];
                rtRecord.Valid = false;
                rtRecords.Remove(rtID);
            }
        }
        
        // === 父子关系相关 ===
        private void ScopeDependencyCheck(string name)
        {
            var spRecords = _model.ScopeRecords;
            var dependenceHash = new HashSet<string>(StringComparer.Ordinal);

            var currentName = name;
            while (currentName != null)
            {
                // 存在检测
                if (!spRecords.TryGetValue(currentName, out var spRecord) || !spRecord.Valid)
                {
                    throw new InvalidOperationException($"[{Tag}]: Scope '{currentName}' does not exist or is invalid.");
                }
                
                // 依赖检测
                if (!dependenceHash.Add(currentName))
                {
                    throw new InvalidOperationException($"[{Tag}]: Scope:{currentName} has circular dependency.");
                }

                currentName = spRecord.Parent;
            }
        }
        private void ScopeRelationshipBuild(ScopeRecord spRecord, string parentName)
        {
            var spRecords = _model.ScopeRecords;
            var spSort = _model.ScopeSort;
            
            var childName = spRecord.Name;
            
            if (parentName != null)
            {
                // 关系记录
                spRecord.Parent = parentName;
                var spParentRecord = spRecords[parentName];
                spParentRecord.Children.Add(childName);
                
                // 添加进组
                var index = spSort.IndexOf(spParentRecord);
                if (index == -1)
                {
                    throw new InvalidOperationException($"[{Tag}]: Scope:{childName} does not exist in scope sort.");
                }
                
                spSort.Insert(index + 1, spRecord);
                _model.SortVersion ++;
                
                return;
            }

            if (spSort.Count > 0)
            {
                throw new InvalidOperationException($"[{Tag}]: Scope only one root scope is allowed.");
            }
            
            spSort.Add(spRecord);
            _model.SortVersion++;
        }
        
        // === 周期实例构建 ===
        private void EntryPointBuild(ScopeRecord spRecord, List<RegisterRecord> entryPoints)
        {
            //注：这里的注册像已经保证全部注册在当前作用域
            entryPoints.Sort((a,b) => a.SerialID.CompareTo(b.SerialID));
            foreach (var rtRecord in entryPoints)
            {
                // 将实例分类装填
                var instance = _spResolveOp.Resolve(spRecord.Name, rtRecord.ImpType);
                var cycle = rtRecord.Lifecycle;
            
                if ((cycle & ELifecycle.Startable) != 0)
                {
                    var startable = (IStartable)instance;
                    spRecord.Startable.Add(startable);
                }

                if ((cycle & ELifecycle.Tickable) != 0)
                {
                    var tickable = (ITickable)instance;
                    spRecord.Tickable.Add(tickable);
                }

                if ((cycle & ELifecycle.LateTickable) != 0)
                {
                    var lateTickable = (ILateTickable)instance;
                    spRecord.LateTickable.Add(lateTickable);
                }

                if ((cycle & ELifecycle.Releasable) != 0)
                {
                    var releasable = (IReleasable)instance;
                    spRecord.Releasable.Add(releasable);
                }
            }
        }
        
        // === 周期驱动参数 ===
        private void StartableTrigger(ScopeRecord spRecord)
        {
            var startables = spRecord.Startable;
            for (var i = 0; i < startables.Count; i++)
            {
                try
                {
                    var startable = startables[i];
                    startable.Start();
                }
                catch(Exception e)
                {
                    FLog.LogError(e.ToString());
                }

            }
        }
        private void ReleasableTrigger(ScopeRecord spRecord)
        {
            var releasables = spRecord.Releasable;
            for (var i =  releasables.Count - 1; i >= 0; i--)
            {
                try
                {
                    var releasable = releasables[i];
                    releasable.Release();
                }
                catch (Exception e)
                {
                    FLog.LogError(e.ToString());
                }

            }
        }
    }
}