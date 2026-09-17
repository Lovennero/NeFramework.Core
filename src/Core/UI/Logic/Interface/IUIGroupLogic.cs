using System.Collections.Generic;

namespace Framework.Core.UI
{
    public interface IUIGroupLogic
    {
        bool GroupValidity(string groupName);
        List<UIRecord> GetGroupUIs(string groupName);

        void PushUI(string groupName, int uiID);
        UIRecord PopUI(string groupName);
    }
}