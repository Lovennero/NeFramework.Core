using System;

namespace Framework.Core.Serialization
{
    internal sealed class SerConfigLifecycleLogic : ISerConfigLifecycleLogic
    {
        private const string Tag = "Ser Lifecycle Logic";
        
        private readonly SerModel _model;
        
        public SerConfigLifecycleLogic(SerModel model)
        {
            _model = model;
        }

        public void Register<T>(string configKey, ISerSerializer<T>  serSerializer, params ISerProcessor[] processors)
        {
            var configs = _model.Records;
            var key = typeof(T).FullName + ":" +  configKey;

            if (configs.TryGetValue(key, out var config))
            {
                throw new InvalidOperationException($"[{Tag}] Config:'{configKey}' is already registered.");
            }
            config = new SerConfigRecord(serSerializer, processors);
            configs.Add(key, config);
        }

        public bool Unregister<T>(string configKey)
        {
            var configs = _model.Records;
            var key = typeof(T).FullName + ":" +  configKey;
            return configs.Remove(key);
        }
    }
}