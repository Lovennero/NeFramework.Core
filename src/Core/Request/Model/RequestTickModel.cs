using System;

namespace Framework.Core.Request
{
    internal sealed class RequestTickModel
    {
        public RequestGroupRecord[] Groups = Array.Empty<RequestGroupRecord>();
        public int GroupsCount;
        
        public RequestRecord[][] GroupRunners = Array.Empty<RequestRecord[]>();
        public int[] GroupRunnersCount =  Array.Empty<int>();
        
        public RequestRecord[][] GroupRetriers = Array.Empty<RequestRecord[]>();
        public int[] GroupRetriersCount =  Array.Empty<int>();
    }
}