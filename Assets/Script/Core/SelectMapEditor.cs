using Script.GamePlay;
using Script.GamePlay.Manager;
using UnityEditor;
using UnityEngine;

namespace Script.Core
{
    [CustomEditor(typeof(GameManager))]
    public class SelectMapEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Editor");
            EditorGUILayout.Space();
            GameManager.CurrentMapIndex = EditorGUILayout.IntField("Select Map:", GameManager.CurrentMapIndex);
            EditorGUILayout.Space();
            if (GUILayout.Button("Select Map"))
            {
                GameManager.Instance.ResetLevel();
            }
        }
    }
}