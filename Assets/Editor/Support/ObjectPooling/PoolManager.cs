using System.Collections.Generic;
using Extensions;
using UnityEngine;

namespace Support
{
    public class PoolManager : MonoSingleton<PoolManager>
    {
        private readonly Dictionary<int, Queue<ObjectInstance>> _poolDictionary = new Dictionary<int, Queue<ObjectInstance>>();

        private ObjectInstance GetObjectToReuse(GameObject prefab)
        {
            var poolKey = prefab.GetInstanceID();

            if (_poolDictionary.ContainsKey(poolKey))
            {
                var objectToReuse = _poolDictionary[poolKey].Dequeue();
                _poolDictionary[poolKey].Enqueue(objectToReuse);

                return objectToReuse;
            }

            this.SafeDebug($"{prefab.transform.name} prefab has not been added to the object pool");
            return null;
        }

        public void CreatePool(GameObject prefab, int poolSize)
        {
            var poolKey = prefab.GetInstanceID();

            var poolHolder = new GameObject(prefab.name + " pool");
            poolHolder.transform.parent = this.transform;

            if (_poolDictionary.ContainsKey(poolKey))
            {
                this.SafeDebug($"{prefab.transform.name} prefab has already been added to the object pool", LogType.Warning);
                return;
            }

            _poolDictionary.Add(poolKey, new Queue<ObjectInstance>());

            for (var i = 0; i < poolSize; i++)
            {
                var instance = new ObjectInstance(Instantiate(prefab));
                instance.SetParent(poolHolder.transform);
                _poolDictionary[poolKey].Enqueue(instance);
            }
        }

        public void ReuseObject(GameObject prefab, Vector3 position)
        {
            var objectToReuse = GetObjectToReuse(prefab);
            objectToReuse.Reuse(position);
        }

        public void ReuseObject(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            var objectToReuse = GetObjectToReuse(prefab);
            objectToReuse.Reuse(position, rotation);
        }
    }
}

