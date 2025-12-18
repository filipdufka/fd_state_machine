using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LinkedMonoBehaviour), true)]
public class LinkedMonoBehaviourEditor : Editor {
    public override void OnInspectorGUI() {
        var t = (LinkedMonoBehaviour)target;

        GUI.color = Color.white;
        if (t.stateActive)
        {
            GUI.color = Color.green;
            EditorGUILayout.LabelField("Active state", GUILayout.Width(100));
        }
        else {
            EditorGUILayout.LabelField("Non-active state", GUILayout.Width(100));
        }

        GUI.color = Color.white;

        base.OnInspectorGUI();
    }

    [MenuItem("Assets/Create/FD State Machine/New State Behaviour")]
    private static void CreateScript()
    {
        string folderPath = GetSelectedFolder();
        // TODO: Create Script Here
    }
    private static string GetSelectedFolder()
    {
        var activeObject = Selection.activeObject;      
        string path = AssetDatabase.GetAssetPath(activeObject);
        
         Path.GetDirectoryName(path);
        Debug.Log(Path.GetDirectoryName(path));
        
        if (string.IsNullOrEmpty(path))
            path = "Assets"; // fallback pokud nic nevybráno

        return path;
    }
}
