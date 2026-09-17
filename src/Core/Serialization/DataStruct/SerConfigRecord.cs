using System.Collections.Generic;

namespace Framework.Core.Serialization
{
    internal sealed class SerConfigRecord
    {
        public ISerSerializer SerSerializer { get; }
        public IReadOnlyList<ISerProcessor> DataProcessors { get; }
        
        public SerConfigRecord(ISerSerializer serSerializer, IReadOnlyList<ISerProcessor> dataProcessors)
        {
            SerSerializer = serSerializer;
            DataProcessors = dataProcessors;
        }
    }
}