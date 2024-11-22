#if UNITY_EDITOR

using System.Linq;
using UnityEditor;

namespace GBGamesPlugin.Editor
{
    [InitializeOnLoad]
    public class DefineSymbols
    {
        public static bool CheckDefine(string define)
        {
            var buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);

            return defines.Contains(define);
        }

        public static void AddDefine(string define)
        {
            var buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);

            if (defines.Contains(define))
                return;

            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, (defines + ";" + define));
        }

        public static void RemoveDefine(string define)
        {
            var buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);

            if (!defines.Contains(define)) return;
            var defineArray = defines.Split(';');

            var updatedDefines = defineArray.Where(d => d != define).ToList();

            var newDefines = string.Join(";", updatedDefines);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, newDefines);
        }

        static DefineSymbols()
        {
            AddDefine("GB_PLUGIN");
        }
    }
}
#endif