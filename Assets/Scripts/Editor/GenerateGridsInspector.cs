using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Created by Vitens on 2020/7/25 15:04:23
/// 
/// Description : 
///     
/// </summary>
[CustomEditor(typeof(GenerateGrids))]
public class GenerateGridsInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Generate"))
        {
            ((GenerateGrids)target).Generate();
        }

        if (GUILayout.Button("Clear"))
        {
            ((GenerateGrids)target).Clear();
        }
        GUILayout.EndHorizontal();
    }
}
