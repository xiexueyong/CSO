#if UNITY_EDITOR
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
                // GenerateCSOFasterCode.GenerateBuilderReferencedTypes();
                GenerateCSOStaticFasterCode.GenerateStaticFasterCode();

                GenerateCSOFasterCode.GenerateCustomFasterCode();
                GenerateCSOFasterCode.GenerateCommonFasterCode();
                AssetDatabase.Refresh(); 
            }
        }
    }
}
#endif