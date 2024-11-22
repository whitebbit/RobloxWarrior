using UnityEngine;

namespace GBGamesPlugin
{
    public sealed class ConditionallyVisibleAttribute : PropertyAttribute
    {
        public string propertyName { get; }
        
        public ConditionallyVisibleAttribute(string propName)
        {
            propertyName = propName;
        }
    }
}
