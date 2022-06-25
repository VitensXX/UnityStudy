using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

/// <summary>
/// Created by Vitens on 2022/6/25 15:44:47
/// 
/// Description : 
///     VertexAnimOnUI面板自定义
/// </summary>
[CustomEditor(typeof(VertexAnimOnUI))]
[CanEditMultipleObjects]
public class VertexAnimOnUIInspector : RawImageEditor
{
    SerializedObject so;
    SerializedProperty _mesh;
    SerializedProperty _animTex;

    protected override void OnEnable() {
        base.OnEnable();
        so = new SerializedObject(target);
        _mesh = so.FindProperty("mesh");
        _animTex = so.FindProperty("animTex");
    }

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI ();
		MeshImage meshImage = target as MeshImage;
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(_mesh);
        if(GUILayout.Button("刷新Mesh", GUILayout.Width(70))){
            meshImage.RefreshMesh();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.PropertyField(_animTex);

        so.ApplyModifiedProperties();
	}
}
