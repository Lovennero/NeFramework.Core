namespace Framework.Core.UI
{
    public interface IUIQueryLogic
    {
        bool HasUI(int serialID);
        UIRecord GetUI(int serialID);
    }
}