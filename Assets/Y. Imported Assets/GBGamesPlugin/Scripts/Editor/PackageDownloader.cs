#if UNITY_EDITOR

using UnityEditor.PackageManager;
using UnityEngine;

namespace GBGamesPlugin.Editor
{
    public class PackageDownloader
    {
        public static bool IsPackageImported(string packageName)
        {
            var packagePath = "Packages/" + packageName;
            var packageInfo = PackageInfo.FindForAssetPath(packagePath);
            return packageInfo != null;
        }

        public static void DownloadPackage(string packageName)
        {
            Client.Add(packageName);
        }
    }
}
#endif