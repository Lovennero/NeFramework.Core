using Framework.Core.FrameLog;

namespace Framework.Core.BTree
{
    public abstract class BTNode
    {
        // === 基本参数 ===
        protected string Name { get; }
        
        // === 运行参数 ===
        public ENodeStatus Status { get; protected set; }
        
        // === 构造函数 ===
        protected BTNode(string name)
        {
            Name = name;
            Status = ENodeStatus.Inactive;
        }
        
        // === 执行方法 ===
        public ENodeStatus Execute()
        {
            if (Status != ENodeStatus.Running)
            {
                OnEnter();
                Status = ENodeStatus.Running;
            }
            Status = Tick();
            if (Status != ENodeStatus.Running)
            {
                OnExit();
            }
            return Status;
        }

        public virtual void Abort()
        {
            if(Status != ENodeStatus.Running) return;
            Status = ENodeStatus.Failure;
        }
        public virtual void Reset()
        {
            Status = ENodeStatus.Inactive;
        }

        
        // === 周期方法 === 
        protected virtual void OnEnter() { }
        protected abstract ENodeStatus Tick();
        protected virtual void OnExit() { }
    }
}