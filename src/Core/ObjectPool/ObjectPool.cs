using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Framework.Core.FrameLog;
using Framework.Core.FrameTime;

namespace Framework.Core.ObjectPool
{
    internal struct ObjectRecord<T>
    {
        public T Object;
        public float LastUseTime;
    }

    public class ObjectPool<T> : IObjectPool<T> where T : class, IObject
    {
        private const string Tag = "Object Pool";

        private float _autoReleaseIntervalTimer;

        private readonly int _initCapacity;
        private readonly SemaphoreSlim _semaphore;

        private readonly HashSet<T> _runningObjects;
        private readonly List<ObjectRecord<T>> _cacheObjects;

        private readonly IObjectProvider<T> _provider;


        public string Name { get; }
        public OverStrategy Strategy { get; }


        public int ActiveCount => _runningObjects.Count;

        public int CacheCount => _cacheObjects.Count;
        public int TotalCount => _runningObjects.Count + _cacheObjects.Count;
        public int Capacity => Strategy == OverStrategy.AutoScale ? TotalCount : _initCapacity;

        public float AutoReleaseInterval { get; }
        public float ExpirationTime { get; }

        public int BlockTimeout { get; set; }



        public ObjectPool(string name, IObjectProvider<T> provider, PoolParams poolParam)
        {
            Name = name;
            
            _provider = provider;
            
            ExpirationTime = poolParam.ExpirationTime;
            _initCapacity = poolParam.Capacity;
            AutoReleaseInterval = poolParam.AutoReleaseInterval;
            Strategy = poolParam.Strategy;
            BlockTimeout = Timeout.Infinite;
            
            _autoReleaseIntervalTimer = AutoReleaseInterval;

            if(poolParam.Strategy == OverStrategy.Block) _semaphore = new SemaphoreSlim(_initCapacity, _initCapacity);
            
            _runningObjects = new HashSet<T>(_initCapacity);
            _cacheObjects = new List<ObjectRecord<T>>(_initCapacity);
        }

        public void OnUpdate(float logicTime, float realTime)
        {
            UpdateClearCache(realTime);
        }

        private void UpdateClearCache(float realTime)
        {
            _autoReleaseIntervalTimer -= realTime;
            if (_autoReleaseIntervalTimer > 0) return;
            _autoReleaseIntervalTimer = AutoReleaseInterval;

            for (var i = _cacheObjects.Count - 1; i >= 0; i--)
            {
                var entry = _cacheObjects[i];
                var interval = FTime.GetRealRuntime() - entry.LastUseTime;
                if (interval < ExpirationTime) continue;
                _cacheObjects.RemoveAt(i);
                entry.Object.OnRelease();
                _provider.Destroy(entry.Object);
            }
        }
        
        public ValueTask<T> SpawnAsync(CancellationToken ct = default)
        {
            if (_cacheObjects.Count > 0) return new ValueTask<T>(SpawnFromCache());

            if (Strategy == OverStrategy.AutoScale || TotalCount < Capacity)
            {
                return new ValueTask<T>(SpawnFromNew());
            }

            if (Strategy == OverStrategy.Block)
            {
                return SpawnWithBlock(ct);
            }

            if (Strategy == OverStrategy.Throw)
            {
                throw new InvalidOperationException($"[{Tag}]:The capacity limit has been exceeded.");
            }

            FLog.LogWarning($"[{Tag}]:The capacity limit has been exceeded.");
            return new ValueTask<T>((T)null);
        }

        private async ValueTask<T> SpawnWithBlock(CancellationToken ct)
        {
            if (!await _semaphore.WaitAsync(BlockTimeout, ct)) return null;
            if (_cacheObjects.Count > 0) return SpawnFromCache();
            return SpawnFromNew();
        }

        private T SpawnFromCache()
        {
            var lastIndex = _cacheObjects.Count - 1;

            var entry = _cacheObjects[lastIndex];
            _cacheObjects.RemoveAt(lastIndex);

            entry.Object.OnSpawn();
            _runningObjects.Add(entry.Object);
            return entry.Object;
        }

        private T SpawnFromNew()
        {
            var po = _provider.Create();
            po.OnSpawn();
            _runningObjects.Add(po);
            return po;
        }
        
        public void UnSpawn(T objectBase)
        {
            if (!_runningObjects.Remove(objectBase))
            {
                FLog.LogWarning($"[{Tag}]:The poolable {objectBase} does not exist.");
                return;
            }

            objectBase.OnDespawn();

            var entry = new ObjectRecord<T>
            {
                Object = objectBase,
                LastUseTime = FTime.GetRealRuntime()
            };
            _cacheObjects.Add(entry);
            _semaphore?.Release(1);
        }

        public void Release()
        {
            foreach (var obj in _runningObjects)
            {
                if (!obj.IsValid) continue;
                obj.OnDespawn();
                obj.OnRelease();
                
                _provider.Destroy(obj);
            }
            _runningObjects.Clear();

            foreach (var entry in _cacheObjects)
            {
                if (!entry.Object.IsValid) continue;
                entry.Object.OnRelease();
                _provider.Destroy(entry.Object);
            }

            _cacheObjects.Clear();
            _semaphore?.Dispose();
        }
    }
}