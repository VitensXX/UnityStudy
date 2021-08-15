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
        UnityEditor.EditorGUILayout.LabelField("The value is " + simpleNode.GetSum());
        simpleNode.sum = simpleNode.GetSum();
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("sum"));

        // Apply property modifications
        serializedObject.ApplyModifiedProperties();
    }
}