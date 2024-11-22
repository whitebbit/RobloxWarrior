using _3._Scripts.Config.Scriptables;

namespace _3._Scripts.Config.Interfaces
{
    public interface IInitializable<in T> 
    {
        public void Initialize(T config);
    }
}