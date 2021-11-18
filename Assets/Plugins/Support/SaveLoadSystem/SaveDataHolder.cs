using System;

namespace Support.SLS
{
    /// <summary>
    /// Class that represents single data entity that can be saved
    /// </summary>
    /// <typeparam name="T">Type of data that will be saved</typeparam>
    [Serializable]
    public class SaveDataHolder<T>
    {
        private T _value;
        public T Value
        {
            get => _value;
            
            set
            {
                _value = value;
                OnValueChanged?.Invoke(_value);
            }
        }

        [field: NonSerialized]
        public event Action<T> OnValueChanged;

        internal SaveDataHolder() { }

        internal SaveDataHolder(T value) => _value = value;
    }
}