using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;

//用于关联你需要自定义的Node
[CustomNodeEditor(typeof(SimpleNode))]

public class SimpleNodeEditor : NodeEditor {
    private SimpleNode simpleNode;

    public override void OnBodyGUI() {
        if (simpleNode == null) simpleNode = target as SimpleNode;

        // Update serialized object's representation
        serializedObject.Update();

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("a"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("b"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("desc"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("duration"));
        UnityEditor.EditorGUILayout.LabelField("The sum value is " + simpleNode.GetSum());
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("sum"));
        UnityEditor.EditorGUILayout.LabelField("The sub value is " + simpleNode.GetSub());
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("sub"));

        // Apply property modifications
        serializedObject.ApplyModifiedProperties();
    }
}