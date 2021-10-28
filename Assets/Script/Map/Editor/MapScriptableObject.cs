using UnityEditor;
using UnityEngine;

namespace Script.Map.Editor
{
    
    public class MapScriptableObject : ScriptableObject
    {
        public int IndexMap;
        public MapInformation m_MapInformation;
    }
    
    [CustomEditor(typeof(MapScriptableObject))]
    public class CreateMapEditor : UnityEditor.Editor
    { 
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Editor"))
            {
                CreateMapEditorWindow.OpenWindow((MapScriptableObject)target);
            }
        }
    }
}
