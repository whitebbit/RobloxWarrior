using System;

namespace _3._Scripts.Detectors.Interfaces
{
    public interface IDetector<out T>
    {
        public bool ObjectsDetected();
        public event Action<T> OnFound;
    }
}