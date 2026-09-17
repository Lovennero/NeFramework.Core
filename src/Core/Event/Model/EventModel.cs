using System.Collections.Generic;

namespace Framework.Core.Event
{
    internal sealed class EventModel
    {
        public readonly Dictionary<string, EventGroupRecord> EventRecords = new();
        public readonly Dictionary<int, EventRecord> Records = new();
        public int SerialID;
    }
}