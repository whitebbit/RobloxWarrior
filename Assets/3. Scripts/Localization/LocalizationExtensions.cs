using System.Threading.Tasks;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.Localization.Tables;

namespace _3._Scripts.Localization
{
    public static class LocalizationExtensions
    {
        public const string DefaultTableName = "Localization";

        public static async Task<string> GetTranslate(this string id, string tableName = "")
        {
            tableName = string.IsNullOrEmpty(tableName) ? DefaultTableName : tableName;
            var tableOperation = LocalizationSettings.StringDatabase.GetTableAsync(tableName);
            await tableOperation.Task;
            return tableOperation.Result[id].LocalizedValue;
        }

        public static void SetReference(this LocalizeStringEvent @event, string id)
        {
            @event.StringReference.SetReference(DefaultTableName, id);
            @event.RefreshString();
        }
        
        public static void SetVariable<T>(this LocalizeStringEvent @event, string valueName, T value = default)
        {
            var stringReference = @event.StringReference;
            if (typeof(T) == typeof(int))
            {
                if (stringReference[valueName] is IntVariable variable)
                {
                    variable.Value = (int)(object)value;
                }
            }
            if (typeof(T) == typeof(string))
            {
                if (stringReference[valueName] is StringVariable variable)
                {
                    variable.Value = (string)(object)value;
                }
            }
            if (typeof(T) == typeof(float))
            {
                if (stringReference[valueName] is FloatVariable variable)
                {
                    variable.Value = (float)(object)value;
                }
            }
            @event.RefreshString();
        }
    }
}