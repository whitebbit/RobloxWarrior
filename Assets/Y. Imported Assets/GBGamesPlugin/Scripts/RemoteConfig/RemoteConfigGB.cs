using System;
using YG;

namespace GBGamesPlugin
{
    public partial class GBGames
    {
        public static T GetRemoteValue<T>(string id, T defaultValue = default)
        {
            var configValue = YandexGame.GetFlag(id);

            if (configValue == null)
            {
                return defaultValue;
            }

            try
            {
                if (typeof(T) == typeof(string))
                {
                    return (T) (object) configValue;
                }

                if (typeof(T) == typeof(int))
                {
                    return (T) (object) int.Parse(configValue);
                }

                if (typeof(T) == typeof(float))
                {
                    return (T) (object) float.Parse(configValue);
                }

                if (typeof(T) == typeof(bool))
                {
                    return (T) (object) bool.Parse(configValue);
                }

                throw new NotSupportedException($"Type {typeof(T)} is not supported");
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}