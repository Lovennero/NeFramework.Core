namespace Framework.Core.BTree
{
    public interface IBTreeLifeLogic
    {
        BTreeRecord BTreeCreate(string key);
        void BTreeRelease(string key);
    }
}