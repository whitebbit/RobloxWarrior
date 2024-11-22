using System;
using GBGamesPlugin;

namespace _3._Scripts.Config
{
    [Serializable]
    public class RemoteConfig<T>
    {
        public string name;
        public T defaultValue;
        public T Value => GBGames.GetRemoteValue(name, defaultValue);
    }
}