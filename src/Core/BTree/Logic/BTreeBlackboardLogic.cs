using System;
using Framework.Core.FrameVariable;

namespace Framework.Core.BTree
{
    internal sealed class BTreeBlackboardLogic : IBTreeBlackboardLogic
    {
        private const string Tag = "BTree Blackboard Logic";

        private BTreeModel BTreeModel { get; }
        
        public BTreeBlackboardLogic(BTreeModel bTreeModel)
        {
            BTreeModel = bTreeModel;
        }

        public void SetValue<T>(string key, string bbKey, T value)
        {
            var records = BTreeModel.BTreeRecords;
            if (!records.TryGetValue(key, out var record) || !record.Valid)
            {
                throw new InvalidOperationException($"[{Tag}]: BTree:{key} does not exist.");
            }
            
            // 缓存查询
            var blackboard = record.Blackboard;
            if (!blackboard.TryGetValue(bbKey, out var variable))
            {
                variable = new FVariable<T>();
                blackboard.Add(bbKey, variable);
            }
            
            // 类型转换
            if (variable is not FVariable<T> typedVariable)
            {
                throw new InvalidOperationException($"[{Tag}]: Value type is wrong.");
            }
            
            // 设置泛型参数
            typedVariable.Value = value;
        }

        public bool RemoveValue(string key, string bbKey)
        {
            var records = BTreeModel.BTreeRecords;
            if (!records.TryGetValue(key, out var record) || !record.Valid)
            {
                throw new InvalidOperationException($"[{Tag}]: BTree:{key} does not exist.");
            }
            
            // 缓存查询
            var blackboard = record.Blackboard;
            return blackboard.Remove(bbKey);
        }

        public bool HasValue<T>(string key, string bbKey)
        {
            var records = BTreeModel.BTreeRecords;
            if (!records.TryGetValue(key, out var record) || !record.Valid)
            {
                throw new InvalidOperationException($"[{Tag}]: BTree:{key} does not exist.");
            }
            
            // 缓存查询
            var blackboard = record.Blackboard;
            if (!blackboard.TryGetValue(bbKey, out var variable)) return false;
            
            // 类型判断
            if (variable is not FVariable<T>)
            {
                throw new InvalidOperationException($"[{Tag}]: Value type is wrong.");
            }
            
            return true;
        }
        
        public T GetValue<T>(string key, string bbKey)
        {
            var records = BTreeModel.BTreeRecords;
            if (!records.TryGetValue(key, out var record) || !record.Valid)
            {
                throw new InvalidOperationException($"[{Tag}]: BTree:{key} does not exist.");
            }
            
            // 不进行值检测
            var blackboard = record.Blackboard;
            var variable = blackboard[bbKey];
            
            // 泛型类型转换
            if (variable is not FVariable<T> typedVariable)
            {
                throw new InvalidOperationException($"[{Tag}]: Value type is wrong.");
            }
            
            // 返回泛型参数
            return typedVariable.Value;
        }

        public bool TryGetValue<T>(string key, string bbKey, out T value)
        {
            var records = BTreeModel.BTreeRecords;
            if (!records.TryGetValue(key, out var record) || !record.Valid)
            {
                throw new InvalidOperationException($"[{Tag}]: BTree:{key} does not exist.");
            }
            
            // 缓存存在检测
            var blackboard = record.Blackboard;
            if (!blackboard.TryGetValue(bbKey, out var variable))
            {
                value = default;
                return false;
            }
            
            // 泛型类型转换
            if (variable is not FVariable<T> typedVariable)
            {
                value = default;
                return false;
            }
            
            value = typedVariable.Value;
            return true;
        }
    }
}