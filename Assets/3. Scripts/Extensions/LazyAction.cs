using System;

namespace _3._Scripts.Extensions
{
    public class LazyAction<T>
    {
        private bool _initialized;
        private event Action<T> Action;
        private T _defaultValue;

        public LazyAction(T defaultValue)
        {
            _defaultValue = defaultValue;
        }

        public void Invoke(T value)
        {
            _initialized = true;
            _defaultValue = value;
            Action?.Invoke(value);
        }

        public void Subscribe(Action<T> action)
        {
            if (_initialized)
            {
                action?.Invoke(_defaultValue);
            }

            Action += action;
        }
    }
}