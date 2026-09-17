namespace Framework.Core.BTree
{
    public abstract class Condition : BTNode
    {
        protected Condition(string name) : base(name) { }
        protected sealed override ENodeStatus Tick()
        {
            return Check() ? ENodeStatus.Success : ENodeStatus.Failure;
        }
        protected abstract bool Check();
    }
}