using UnityEditor;
using UnityEngine;

namespace HYDAC.QR
{
    [CustomEditor(typeof(QRTranslator))]
    public class QRTranslatorEditor : Editor
    {
        public Texture HydacLogo;
        [TextArea]
        public string Documentation;

        private QRTranslator myScript;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            myScript = (QRTranslator)target;

            GUILayout.Space(20);
            GUILayout.Label(new GUIContent(HydacLogo, Documentation));
            GUILayout.Space(20);
            GUILayout.Label(new GUIContent("DEBUG CONTROLS\n=============="));
            GUILayout.Space(20);

            if (GUILayout.Button("\nFLUSH DICTIONARY\n----------------\n"))
            {
                myScript.FlushQRDictionary();
            }
        }
    }
}
