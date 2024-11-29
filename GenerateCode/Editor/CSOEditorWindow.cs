#if UNITY_EDITOR
using System.IO;
using UnityEngine;
using UnityEditor;

namespace CSOEngine.GenerateCode.Editor
{
    public class CSOEditorWindow : EditorWindow
    {
        [MenuItem("Tools/CSO")]
        public static void ShowWindow()
        {
            GetWindow<CSOEditorWindow>("CSO Editor");
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Generate CSO Faster"))
            {
                string directoryPath = "Assets/Scripts/App/APPCSO/CSOGenerated";
                if (Directory.Exists(directoryPath))
                {
                    Directory.Delete(directoryPath,true);
                }
                
                GenerateCSOStaticFasterCode.GenerateStaticFasterCode();
                AutoGenerateCSOFasterCode.TryGenerateBuilderReferencedTypes();

                // GenerateCSOFasterCode.GenerateCustomFasterCode();
                // GenerateCSOFasterCode.GenerateCommonFasterCode();
                // GenerateCSOFasterViewCode.GenerateCommonFasterViewCode();
                // GenerateCSOFasterViewCode.GenerateCustomFasterViewCode();
                AssetDatabase.Refresh(); 
            }
        }
    }
}
#endif