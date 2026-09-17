namespace Framework.Core.Entity
{
    internal sealed class EntityPoolReleaseOperator
    {
        private readonly EntityModel _model;
        private readonly IEntityPoolHelper _helper;
        private readonly IEntityProvider _provider;
        
        public EntityPoolReleaseOperator(
            EntityModel model,
            IEntityPoolHelper helper,
            IEntityProvider provider)
        {
            _model = model;
            _helper = helper;
            _provider = provider;
        }

        public void Execute(EntityPoolRecord record)
        {
            _helper.ReleasePool(record.PoolName);
            record.IsValid = false;
            
            _model.Pools.Remove(record.PoolName);
            _model.PoolSorts.Remove(record);
            
            _provider.UnLoadAsset(record.PoolName);
        }
    }
}