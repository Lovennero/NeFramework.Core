
namespace Framework.Core.Sound
{
    internal sealed class SoundGeneralQueryLogic:ISoundGeneralQueryLogic
    {
        private const string Tag = "Sound General Query Logic";
        
        private readonly SoundModel _model;
        
        public SoundGeneralQueryLogic(SoundModel model)
        {
            _model = model;
        }
        
        public bool HasGroup(string groupName)
        {
            var groups = _model.GroupRecords;
            return groups.ContainsKey(groupName);
        }

        public SoundGroupRecord GetGroup(string groupName)
        {
            var groups = _model.GroupRecords;
            groups.TryGetValue(groupName, out var group);
            return group;
        }

        public bool HasSound(int id)
        {
            var records = _model.Records;
            return records.ContainsKey(id);
        }

        public SoundRecord GetSound(int id)
        {
            var records = _model.Records;
            records.TryGetValue(id, out var record);
            return record;
        }
    }
}