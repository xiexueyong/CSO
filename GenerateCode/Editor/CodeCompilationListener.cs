using System;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
public class CodeCompilationListener : AssetPostprocessor
{
    public static bool IsBuidlerChanged
    {
        get => EditorPrefs.GetBool("IsBuilderChanged", false);
        private set => EditorPrefs.SetBool("IsBuilderChanged", value);
    }
    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        if (!IsBuidlerChanged) {
            Action<string> checkAsset = (asset) => {
                if (asset.EndsWith(".cs") && asset.Contains("/Builder/")){ //简单处理 减少触发编译检查次数
                    IsBuidlerChanged = true;
                    Debug.Log("generate builder faster flag");
                }
            };
            importedAssets.ForEach(checkAsset);
            deletedAssets.ForEach(checkAsset);
        }
    }
    
    [InitializeOnLoad]
    public class DllCompilationListener
    {
        static DllCompilationListener()
        {
            AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;
        }
        private static void OnAfterAssemblyReload()
        {
            if (IsBuidlerChanged) {
                // AutoGenerateCSOFasterCode.TryGenerateBuilderReferencedTypes();
            }
            IsBuidlerChanged = false;
        }
    }
}
#endif


