using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class GameObjectCustom : EditorWindow
{
    [MenuItem("Tools/GameObject Custom")]
    public static void Open()
    {
        GetWindow<GameObjectCustom>();
    }

    public List<Transform> trsRandomRotate = new List<Transform>();
    public List<GameObject> objChangeNames = new List<GameObject>();
    public string nameChange;

    private void OnGUI()
    {
        SerializedObject obj = new SerializedObject(this);
        EditorGUILayout.PropertyField(obj.FindProperty("trsRandomRotate"));
        EditorGUILayout.PropertyField(obj.FindProperty("objChangeNames"));
        EditorGUILayout.PropertyField(obj.FindProperty("nameChange"));
        EditorGUILayout.BeginVertical("box");
        DrawButton();
        EditorGUILayout.EndVertical();
        obj.ApplyModifiedProperties();
    }

    void DrawButton()
    {
        if (GUILayout.Button("Random Rotate"))
        {
            RandomRotate();
        }
        if (GUILayout.Button("Change Name"))
        {
            ChangeName();
        }
    }

    private void ChangeName()
    {
        for (int i = 0; i < objChangeNames.Count; i++)
        {
            objChangeNames[i].name = nameChange+"_" + i;
        }
    }

    private void RandomRotate()
    {
        for (int i = 0; i < trsRandomRotate.Count; i++)
        {
            trsRandomRotate[i].eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
        }
    }
}

#endif
