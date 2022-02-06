using System;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

using HYDACDB.INFO;


namespace HYDACDB.PRO.Editor
{
    [CustomEditor(typeof(ProductFModule))]
    public class ModuleEditor : UnityEditor.Editor
    {
        public Texture HydacLogo;
        [TextArea]
        public string Documentation;

        private DefaultAsset _folderToSaveTo = null;
        private ProductFModule myScript;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            myScript = (ProductFModule)target;

            if ((myScript.Info as SModuleInfo).isStatic) return;


            GUILayout.Label(new GUIContent(HydacLogo, Documentation));
            GUILayout.Label(new GUIContent("DEBUG CONTROLS\n=============="));

            ModuleIntialization();

            GUILayout.Space(20);

            if (GUILayout.Button("\nEXPLODE\n-------\n"))
            {
                myScript.ToggleExplosion(true);
            }
            if (GUILayout.Button("\nIMPLODE\n-------\n"))
            {
                myScript.ToggleExplosion(false);
            }
        }
        
        
        [MenuItem ("Window/Folder Selection Example")]
        public static void  ShowWindow () 
        {
            EditorWindow.GetWindow(typeof(ProductInfo));
        }
     
        private void ModuleIntialization() 
        {
            if (_folderToSaveTo == null)
            {
                EditorGUILayout.HelpBox(
                    "Folder not Specified!", 
                    MessageType.Error, 
                    true);
            }

            _folderToSaveTo = (DefaultAsset)EditorGUILayout.ObjectField(
                "Save Location:",
                _folderToSaveTo,
                typeof(DefaultAsset),
                false);
            
            if (_folderToSaveTo == null) return;

            GUILayout.Space(5);

            InitializeModule();

            UpdateModule();
        }

        private void InitializeModule()
        {
            if (GUILayout.Button("\nINITIALISE MODULE\n-----------------\n"))
            {
                if (myScript.Info != null) return;

                // Create Module Info
                SModuleInfo modInfo = ScriptableObject.CreateInstance<SModuleInfo>();

                modInfo.ID = Convert.ToInt32(myScript.transform.name.Substring(0, 2));
                modInfo.iname = myScript.name.Substring(3);
                modInfo.description = "Module Description";

                //EditorUtility.SetDirty(modInfo);

                string fileName = "MInfo_" + modInfo.ID.ToString("D2") + "_" + modInfo.iname + ".asset";
                modInfo.name = fileName;

                string folderURL = AssetDatabase.GetAssetPath(_folderToSaveTo.GetInstanceID());
                string fileURL = folderURL + "/" + fileName;

                AssetDatabase.CreateAsset(modInfo, fileURL);

                myScript.SetPartInfo(modInfo);
            }
        }

        private void UpdateModule()
        {
            if (GUILayout.Button("\nUPDATE MODULE\n-------------\n"))
            {
                // Check if prefab is set up previously
                if (!myScript.UpdateSubModules()) return;

                // Previous sub module ID
                int previousId = -1;

                // Loop Variables
                int id;
                string name;
                SSubModuleInfo subModInfo = default;
                List<SSubModuleInfo> subModuleInfos = new List<SSubModuleInfo>();

                for (int i = 0; i < myScript.SubModulesCount; i++)
                {
                    //Debug.Log("Previous ID: " + previousId + " , Current ID: " + i);

                    var subModuleTransform = myScript.RootTransform.GetChild(i);
                    var subModule = myScript.SubModules[i];

                    // Get ID of sub module
                    try
                    {
                        // Get ID of sub module
                        id = Convert.ToInt32(subModuleTransform.name.Substring(0, 2));

                        // If ID is same then set the previous subModule and skip to next iteration
                        if (previousId == id && id != 0)
                        {
                            Debug.Log("Same sub module id: " + subModuleTransform.name);

                            subModule.SetPartInfo(subModInfo);
                            continue;
                        }
                    }
                    catch
                    {
                        Debug.LogError("FocusedModuleEditor#--------Error: " + subModuleTransform.name + " - ID");
                        return;
                    }

                    // Get name of submodule
                    name = subModuleTransform.name.Substring(3);

                    subModInfo = ScriptableObject.CreateInstance<SSubModuleInfo>();
                    subModInfo.ID = id;
                    subModInfo.iname = name;

                    // Check if submodule info is assigned
                    if (subModule.Info == null)
                    {
                        subModInfo.description = "To be filled later";
                    }
                    else
                    {
                        // Copy description of the sub module info
                        subModInfo.description = subModule.Info.description;

                        // Destroy and delete previous one if already present
                        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(subModule.Info.GetInstanceID()));
                    }

                    // Save submodule info to location and set property
                    string fileName = "SInfo_" + subModInfo.ID + "_" + subModInfo.iname + ".asset";
                    subModInfo.name = fileName;

                    string folderURL = AssetDatabase.GetAssetPath(_folderToSaveTo.GetInstanceID());
                    string fileURL = folderURL + "/" + fileName;

                    AssetDatabase.CreateAsset(subModInfo, fileURL);

                    subModule.SetPartInfo(subModInfo);
                    subModuleInfos.Add(subModInfo);

                    previousId = i;
                }

                // Get the submodules on the Module info
                (myScript.Info as SModuleInfo).SetSubModules(subModuleInfos.ToArray());
            }
        }
    }
}