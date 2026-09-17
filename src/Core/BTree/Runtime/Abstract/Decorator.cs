namespace Framework.Core.BTree
{
    public abstract class Decorator : BTNode
    {
        protected BTNode Child { get; }
        protected Decorator(string name, BTNode child) : base(name)
        {
            Child = child;
        }
    }
}