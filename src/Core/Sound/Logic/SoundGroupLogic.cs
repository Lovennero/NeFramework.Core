using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.Core.Sound
{
    internal sealed class SoundGroupLogic:ISoundGroupLogic
    {
        private const string Tag = "Sound Group Control Logic";

        // === 内部数据 ===
        private readonly SoundModel _model;
        
        // === 原子操作 ===
        private readonly SoundRecycleOperator _recycleOperator;
        
        public SoundGroupLogic(SoundModel model,SoundRecycleOperator recycleOperator)
        {
            _model = model;
            _recycleOperator = recycleOperator;
        }

        public float GetGroupVolume(string groupName)
        {
            var groupRecords = _model.GroupRecords;
            if (!groupRecords.TryGetValue(groupName, out var groupRecord))
            {
                throw new InvalidOperationException($"[{Tag}]:Group {groupName} does not exist.");
            }
            
            return groupRecord.SoundParams.Volume;
        }
        
        public void SetGroupVolume(string groupName, float volume)
        {
            var records = _model.Records;
            var groupRecords = _model.GroupRecords;
            
            if (!groupRecords.TryGetValue(groupName , out var groupRecord))
            {
                throw new InvalidOperationException($"[{Tag}]:Group {groupName} does not exist.");
            }
            
            var members = groupRecord.Members;

            var memberIds = new int[members.Count];
            members.CopyTo(memberIds);
            foreach (var id in memberIds)
            {
                var record = records[id];
                record.SoundParams.Volume = volume;
                record.Sound.RefreshParams();
            }
        }
        
        public bool GetGroupMute(string groupName)
        {
            var groupRecords = _model.GroupRecords;
            if (!groupRecords.TryGetValue(groupName, out var groupRecord))
            {
                throw new InvalidOperationException($"[{Tag}]:Group {groupName} does not exist.");
            }
            
            return groupRecord.SoundParams.Mute;
        }
        
        public void SetGroupMute(string groupName, bool mute)
        {
            var records = _model.Records;
            var groupRecords = _model.GroupRecords;
            
            if (!groupRecords.TryGetValue(groupName, out var groupRecord))
            {
                throw new InvalidOperationException($"[{Tag}]:Group {groupName} does not exist.");
            }
            
            var members = groupRecord.Members;

            var memberIds = new int[members.Count];
            members.CopyTo(memberIds);
            foreach (var id in memberIds)
            {
                var record = records[id];
                record.SoundParams.Mute = mute;
                record.Sound.RefreshParams();
            }
        }

        public float GetGroupPitch(string groupName)
        {
            var groupRecords = _model.GroupRecords;
            if (!groupRecords.TryGetValue(groupName, out var groupRecord))
            {
                throw new InvalidOperationException($"[{Tag}]:Group {groupName} does not exist.");
            }
            
            return groupRecord.SoundParams.Pitch;
        }
        
        public void SetGroupPitch(string groupName, float pitch)
        {
            var records = _model.Records;
            var groupRecords = _model.GroupRecords;
            
            if (!groupRecords.TryGetValue(groupName,  out var groupRecord))
            {
                throw new InvalidOperationException($"[{Tag}]:Group {groupName} does not exist.");
            }
            
            var members = groupRecord.Members;

            var memberIds = new int[members.Count];
            members.CopyTo(memberIds);
            foreach (var id in memberIds)
            {
                var record = records[id];
                record.SoundParams.Pitch = pitch;
                record.Sound.RefreshParams();
            }
        }
        
        public List<SoundRecord> GetGroupRecords(string groupName)
        {
            var records = _model.Records;
            var groups = _model.GroupRecords;
            if (!groups.TryGetValue(groupName, out var groupRecord))
            {
                throw new InvalidOperationException($"[{Tag}]: Group {groupName} Members infos does not exist.");
            }

            var list = new List<SoundRecord>(groupRecord.Members.Count);
            foreach (var memberID in groupRecord.Members)
            {
                if (!records.TryGetValue(memberID, out var record))
                {
                    throw new InvalidOperationException($"[{Tag}]:Sound (ID:{memberID}) does not exist.)");
                }
                
                list.Add(record);
            }
            
            //即时快照
            list.Sort((a, b) => a.Priority.CompareTo(b.Priority));
            return list;
        }
        
        public async ValueTask StopGroupAllSound(string groupName,float fadeInTime)
        {
            var records = _model.Records;
            var groupRecords = _model.GroupRecords;

            if (!groupRecords.TryGetValue(groupName, out var groupRecord))
            {
                throw new InvalidOperationException($"[{Tag}]: Group:{groupName} does not exist.");
            }

            var members = groupRecord.Members;

            var memberIds = new int[members.Count];
            members.CopyTo(memberIds);

            var tasks = new List<Task>(memberIds.Length);
            foreach (var id in memberIds)
            {
                var record = records[id];
                tasks.Add(StopTask(record, fadeInTime));
            }

            await Task.WhenAll(tasks);
        }

        private async Task StopTask(SoundRecord record,float fadeInTime)
        {
            await record.Sound.Stop(fadeInTime);
            _recycleOperator.Execute(record);
        }
    }
}