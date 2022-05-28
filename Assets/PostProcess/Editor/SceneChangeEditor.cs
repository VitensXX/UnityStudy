using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SceneChange))]
public class SceneChangeEditor : Editor
{
    public override void OnInspectorGUI ()
	{
        base.OnInspectorGUI();
        SceneChange sc = ((SceneChange)target);
        if(GUILayout.Button("RePlay")){
            sc.RePlay();
        }

        GUILayout.Label(sc.tick.ToString());
	}
}
