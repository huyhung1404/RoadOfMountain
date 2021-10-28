using System.IO;
using Script.GamePlay.Player;
using UnityEditor;
using UnityEngine;

namespace Script.Map.Editor
{
    public class CreateMapEditorWindow : EditorWindow
    {
        private static SerializedObject m_SerializedObject;
        private static MapScriptableObject m_MapObject;
        private static GameObject hero;
        private Vector3 m_Position;
        private static Vector3 m_CameraOffset;
        private int forward = -1;
        private int back = -1;
        private int left = -1;
        private int right = -1;
        
        public static void OpenWindow(MapScriptableObject _object)
        {
            CreateMapEditorWindow window = GetWindow<CreateMapEditorWindow>("Create Map");
            window.minSize = new Vector2(300, 300);
            m_MapObject = _object;
            m_CameraOffset = _object.m_MapInformation.cameraOffset.ToVector3();
            m_SerializedObject = new SerializedObject(_object);
            hero = FindObjectOfType<PlayerMovement>().gameObject;
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("Camera Offset",EditorStyles.boldLabel);
            m_CameraOffset = EditorGUILayout.Vector3Field("Camera Offset", m_CameraOffset);
            m_MapObject.m_MapInformation.cameraOffset = m_CameraOffset.FromVector3();
            GUILayout.Space(10);
            GUILayout.Label("Add Position",EditorStyles.boldLabel);
            m_Position = EditorGUILayout.Vector3Field("Position", m_Position);
            GUILayout.Space(10);
            if (GUILayout.Button("Get"))
            {
                m_Position = hero.transform.position;
            }

            forward = EditorGUILayout.IntField("Forward", forward);
            back = EditorGUILayout.IntField("Back", back);
            left = EditorGUILayout.IntField("Left", left);
            right = EditorGUILayout.IntField("Right", right);
            
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add"))
            {
                m_MapObject.m_MapInformation.mapPositions.Add(new MapPosition(m_Position.FromVector3(),forward,back,left,right));
                m_SerializedObject = new SerializedObject(m_MapObject);
                m_Position = Vector3.zero;
                forward = -1;
                back = -1;
                left = -1;
                right = -1;
            }

            if (GUILayout.Button("Delete"))
            {
                m_MapObject.m_MapInformation.mapPositions.RemoveAt(m_MapObject.m_MapInformation.mapPositions.Count -1);
                m_SerializedObject = new SerializedObject(m_MapObject);
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
                
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Create Map"))
            { 
                WriteToFile(JsonUtility.ToJson(m_MapObject.m_MapInformation));
            }
            m_MapObject.IndexMap = EditorGUILayout.IntField(m_MapObject.IndexMap);
            GUILayout.EndHorizontal();
            if (GUILayout.Button("Load Map"))
            { 
                TextAsset textMap = Resources.Load<TextAsset>($"Map/Map{m_MapObject.IndexMap}/Map{m_MapObject.IndexMap}");
                m_MapObject.m_MapInformation = JsonUtility.FromJson<MapInformation>(textMap.text);
                m_SerializedObject = new SerializedObject(m_MapObject);
            }
            if (GUILayout.Button("Delete All"))
            {
                m_MapObject.m_MapInformation.mapPositions.Clear();
                m_SerializedObject = new SerializedObject(m_MapObject);
            }

            EditorGUILayout.PropertyField(m_SerializedObject.FindProperty("m_MapInformation"));
        }

        private void WriteToFile(string json)
        {
            FileStream fileStream = new FileStream(GetFilePath(), FileMode.Create);
            using StreamWriter writer = new StreamWriter(fileStream);
            writer.Write(json);
            Debug.Log("Lưu thành công");
        }
        
        private string GetFilePath()
        {
            return Application.dataPath + $"/Resources/Map/Map{m_MapObject.IndexMap}/Map{m_MapObject.IndexMap}.txt";
        }
    }
}
