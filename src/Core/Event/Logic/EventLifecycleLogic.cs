using System;

namespace Framework.Core.Event
{
    internal sealed class EventLifecycleLogic : IEventLifecycleLogic
    {
        private const string Tag = "Event Logic";

        private readonly EventModel _eventModel;
        
        public EventLifecycleLogic(EventModel eventModel)
        {
            _eventModel = eventModel;
        }
        
        public EventRecord Subscribe<T>(string eventKey, EventHandlerAsync<T> handler) where T : IMsg
        {
            // 所需参数
            var type = typeof(T);
            var key = eventKey + ":" + type.FullName;

            // 数据集合
            var records = _eventModel.Records;
            var eventRecords = _eventModel.EventRecords;
            
            // 合法检测
            if (!eventRecords.TryGetValue(key, out var eventRecord))
            {
                // 自动创建小组
                eventRecord = new EventGroupRecord(eventKey, type);
                eventRecords[key] = eventRecord;
                eventRecord.Validity = true;
            }

            // 参数包装
            var serialID = _eventModel.SerialID++;
            var record = new EventRecord(serialID, type, eventKey, handler);
            
            // 数据同步
            records.Add(serialID, record);
            eventRecord.Members.Add(serialID);
            record.Validity = true;
            
            return record;
        }
        
        public void Unsubscribe<T>(string eventKey, EventHandlerAsync<T> handler) where T : IMsg
        {
            // 所需参数
            var type = typeof(T);
            var key = eventKey + ":" + type.FullName;
            
            // 数据集合
            var records = _eventModel.Records;
            var eventRecords = _eventModel.EventRecords;

            // 合法检测
            if (!eventRecords.TryGetValue(key, out var eventRecord))
            {
                throw new InvalidOperationException($"[{Tag}]: Group:{eventKey} does not exist.");
            }

            // 拿到索引
            var members = eventRecord.Members;
            for (var i = records.Count -1; i >= 0; i--)
            {
                // 参数准备
                var member = members[i];
                var record = records[member];
                
                // 合法检测
                if(!record.Validity) continue;
                if (record.Handler is not EventHandlerAsync<T> tmpHandler)
                {
                    throw new InvalidOperationException($"[{Tag}]: Record handler is not EventHandlerAsync<T>.");
                }
                if (tmpHandler != handler) continue;
                
                // 实现移除
                members.RemoveAt(i);
                records.Remove(member);
                record.Validity = false;
                return;
            }
            
            // 自动移除事件组
            if (members.Count == 0)
            {
                eventRecords.Remove(key);
                return;
            }
            
            throw new InvalidOperationException($"[{Tag}]: Handler does not exist.");
        }

        public void Unsubscribe<T>(string eventKey, int id) where T : IMsg
        {
            // 所需参数
            var type = typeof(T);
            var key = eventKey + ":" + type.FullName;
            
            // 数据集合
            var records = _eventModel.Records;
            var eventRecords = _eventModel.EventRecords;

            // 合法检测
            if (!eventRecords.TryGetValue(key, out var eventRecord))
            {
                throw new InvalidOperationException($"[{Tag}]: Group:{eventKey} does not exist.");
            }
            
            // 执行移除
            var members = eventRecord.Members;
            if (!members.Remove(id))
            {
                throw new InvalidOperationException($"[{Tag}]: Handler:{id} does not exist.");
            }
            
            var record = records[id];
            records.Remove(id);
            record.Validity = false;
        }
    }
}