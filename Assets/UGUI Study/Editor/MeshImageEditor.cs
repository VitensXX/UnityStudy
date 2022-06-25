using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;
 
[CustomEditor(typeof(MeshImage), true)]
[CanEditMultipleObjects]
public class MeshImageEditor : RawImageEditor {
    
    SerializedObject so;
    SerializedProperty _mesh;

    protected override void OnEnable() {
        base.OnEnable();
        so = new SerializedObject(target);
        _mesh = so.FindProperty("mesh");
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

        so.ApplyModifiedProperties();
	}

    [MenuItem("GameObject/UI/Mesh Image")]
    static void CreateBtnLanter()
    {
        for (int i = 0; i < Selection.objects.Length; i++)
        {
            GameObject obj = Selection.objects[i] as GameObject;
            GameObject meshImageGo = new GameObject("MeshImage");
            meshImageGo.AddComponent<MeshImage>();
            meshImageGo.transform.parent = obj.transform;
            RectTransform rect = meshImageGo.GetComponent<RectTransform>();
            rect.anchoredPosition3D = Vector3.zero;
            meshImageGo.transform.localScale = new Vector3(100f, 100f, 100f);
            rect.sizeDelta = new Vector2(1,1);
        }
    }

 
}