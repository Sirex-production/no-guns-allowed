using UnityEngine;

namespace Support
{
    /// <summary>
    /// PoolObject is an abstract class that one should inherit from if they want to implement a specific behaviour for an object, 
    /// besides simply changing its position and/or rotation.
    /// </summary>
    public abstract class PoolObject : MonoBehaviour
    {
        public abstract void OnObjectReuse();
    }

    /// <summary>
    /// This class is used to extend GameObject class specifically for object pooling
    /// </summary>
    public class ObjectInstance
    {
        private readonly PoolObject _poolObjectComponent;
        private readonly bool _hasPoolObjectComponent;
        private readonly GameObject _gameObject;
        private readonly Transform _transform;

        public ObjectInstance(GameObject objectInstance)
        {
            _gameObject = objectInstance;
            _transform = _gameObject.transform;
            _gameObject.SetActive(false);

            if (_gameObject.GetComponent<PoolObject>())
            {
                _poolObjectComponent = _gameObject.GetComponent<PoolObject>();
                _hasPoolObjectComponent = true;
            }
            else
            {
                _poolObjectComponent = null;
                _hasPoolObjectComponent = false;
            }
        }

        public void SetParent(Transform parent)
        {
            _transform.parent = parent;
        }

        public void Reuse(Vector3 position)
        {
            _gameObject.SetActive(true);
            _transform.position = position;

            if (_hasPoolObjectComponent)
                _poolObjectComponent.OnObjectReuse();
        }

        public void Reuse(Vector3 position, Quaternion rotation)
        {
            Reuse(position);
            _transform.rotation = rotation;
        }
    }
}

