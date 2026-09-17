
using System.Collections.Generic;

namespace Framework.Core.Serialization
{
    internal sealed class SerModel
    {
        public readonly Dictionary<string, SerConfigRecord> Records = new();
    }
}