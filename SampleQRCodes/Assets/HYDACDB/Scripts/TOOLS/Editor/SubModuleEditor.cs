using System;
using UnityEditor;
using UnityEngine;

using HYDACDB.INFO;

namespace HYDACDB.PRO.Editor
{
    [CustomEditor(typeof(ProductSubModule))]
    public class SubModuleEditor : UnityEditor.Editor
    {
        public string ID = "ID:";
        public string Description = "Description:";
        
        private DefaultAsset _folderToSaveTo = null;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            ProductSubModule myScript = (ProductSubModule)target;

            if (myScript.Info != null)
                return;
            
            GUILayout.Space(20);
            
            GUILayout.Label("Use the below GUI create a SInfo if not created");
            

            ID = GUILayout.TextField(ID, 2);
            Description = GUILayout.TextArea(Description, 500);
            
            OnGUI();

            if (GUILayout.Button("Create SubModule Info"))
            {
                SSubModuleInfo info = ScriptableObject.CreateInstance<SSubModuleInfo>();
                
                var id = myScript.transform.name.Substring(0, 2);
                info.ID = Convert.ToInt32(id);

                var name = myScript.transform.name.Substring(2);
                info.iname = name;

                info.description = Description;
                
                EditorUtility.SetDirty(info);

                string fileName =  "SInfo_" + info.ID + "_" + info.iname + ".asset";
                string folderURL = AssetDatabase.GetAssetPath(_folderToSaveTo.GetInstanceID());
                string fileURL = folderURL + fileName;
                
                AssetDatabase.CreateAsset(info, fileURL);

                myScript.SetPartInfo(info);
            }
        }
        
        
        [MenuItem ("Window/Folder Selection Example")]
        public static void  ShowWindow () 
        {
            EditorWindow.GetWindow(typeof(ProductInfo));
        }
     
        void OnGUI () 
        {
            _folderToSaveTo = (DefaultAsset)EditorGUILayout.ObjectField(
                "Folder to save to", 
                _folderToSaveTo, 
                typeof(DefaultAsset), 
                false);
 
            if (_folderToSaveTo != null) {
                EditorGUILayout.HelpBox(
                    "Valid folder! Name: " + _folderToSaveTo.name, 
                    MessageType.Info, 
                    true);
            }
            else
            {
                EditorGUILayout.HelpBox(
                    "Not valid!", 
                    MessageType.Warning, 
                    true);
            }
        }
    }
}