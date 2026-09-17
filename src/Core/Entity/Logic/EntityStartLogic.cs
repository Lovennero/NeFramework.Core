namespace Framework.Core.Entity
{
    internal class EntityStartLogic : IEntityStartLogic
    {
        private const string Tag = "Entity Start Logic";

        private readonly IEntityStartHelper _helper;
        
        public EntityStartLogic(IEntityStartHelper helper)
        {
            _helper = helper;
        }
        
        public void Start()
        {
            _helper.InitEntityRoot();
        }
    }
}