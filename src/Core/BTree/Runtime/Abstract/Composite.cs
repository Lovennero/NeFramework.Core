using System.Collections.Generic;

namespace Framework.Core.BTree
{
    public abstract class Composite : BTNode
    {
        protected IReadOnlyList<BTNode> Children { get; }

        protected Composite(string name, params BTNode[] children) : base(name)
        {
            Children = new List<BTNode>( children );
        }
        
        // === 执行方法 ===
        public override void Reset()
        {
            foreach (var child in Children)
            {
                child.Reset();
            }
            base.Reset();
        }
    }
}