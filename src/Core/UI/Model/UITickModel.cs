using System;

namespace Framework.Core.UI
{
    internal sealed class UITickModel
    {
        public UIGroupRecord[] Groups = Array.Empty<UIGroupRecord>();
        public int GroupCount;
        
        public UIRecord[][] GroupUIs = Array.Empty<UIRecord[]>();
        public int[] GroupUICounts = Array.Empty<int>();
        
        public UIPoolRecord[] Pools = Array.Empty<UIPoolRecord>();
        public int PoolCount;
    }
}