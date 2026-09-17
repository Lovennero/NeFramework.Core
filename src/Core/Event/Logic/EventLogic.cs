using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework.Core.FrameLog;

namespace Framework.Core.Event
{
    internal sealed class EventLogic : IEventLogic
    {
        private const string Tag = "Event Logic";

        private readonly EventModel _eventModel;
        
        public EventLogic(EventModel eventModel)
        {
            _eventModel = eventModel;
        }

        public async ValueTask PublishSeq<T>(string eventKey, T msg) where T : IMsg
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
                FLog.LogWarning($"[{Tag}]: Event:{eventKey} does not exist.");
                return;
            }
            
            // 触发事件
            var members = eventRecord.Members;
            var tmpMembers = members.ToArray();
            
            for (var i = 0; i < tmpMembers.Length; i++)
            {
                var member = tmpMembers[i];
                
                if (!records.TryGetValue(member, out var record)) continue;
                if (record.Handler is not EventHandlerAsync<T> handler)
                {
                    throw new InvalidOperationException($"[{Tag}]: Event:{eventKey} Handler type mismatch.");
                }
                
                try
                {
                    await handler.Invoke(msg);
                }
                catch (Exception e)
                {
                    FLog.LogError(e.ToString());
                }
            }
        }

        public async ValueTask PublishCon<T>(string eventKey, T msg) where T : IMsg
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
                FLog.LogWarning($"[{Tag}]: Event:{eventKey} does not exist.");
                return;
            }
            
            // 触发事件
            var members = eventRecord.Members;
            var tmpMembers = members.ToArray();

            var tasks = new List<Task>();
            foreach (var member in tmpMembers)
            {
                if (!records.TryGetValue(member, out var record)) continue;
                if (record.Handler is not EventHandlerAsync<T> handler)
                {
                    throw new InvalidOperationException($"[{Tag}]: Event:{eventKey} Handler type mismatch.");
                }
                
                var task = handler.Invoke(msg).AsTask();
                tasks.Add(task);
            }
            
            await Task.WhenAll(tasks);
        } 
        
        public void PublishSync<T>(string eventKey, T msg) where T : IMsg
        {
            var key = eventKey + ":" + typeof(T).FullName;

            var records = _eventModel.Records;
            if (!_eventModel.EventRecords.TryGetValue(key, out var eventRecord))
            {
                FLog.LogWarning($"[{Tag}]: Event:{eventKey} does not exist.");
                return;
            }

            var tmpMembers = eventRecord.Members.ToArray();
            foreach (var member in tmpMembers)
            {
                if (!records.TryGetValue(member, out var record)) continue;
                if (record.Handler is not EventHandlerAsync<T> handler)
                {
                    throw new InvalidOperationException($"[{Tag}]: Event:{eventKey} Handler type mismatch.");
                }

                try
                {
                    var task = handler.Invoke(msg);
                    if (!task.IsCompleted)
                    {
                        FLog.LogError($"[{Tag}]: Event:{eventKey} handler is not sync，Please use PublishSeq or PublishCon.");
                        continue;
                    }

                    task.GetAwaiter().GetResult();
                }
                catch (Exception e)
                {
                    FLog.LogError(e.ToString());
                }
            }
        }
    }
}