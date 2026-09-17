
using System.Collections.Generic;

namespace Framework.Core.Network
{
    internal sealed class NetModel
    {
        public readonly Dictionary<string, NetChannelRecord> NetChannels = new();
    }
}