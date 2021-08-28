using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;

[CustomNodeEditor(typeof(BaseNode))]

public class BaseNodeEditor : NodeEditor {

    private BaseNode _baseNode;

    // private void OnEnable() {
    //     _baseNode = target as BaseNode;
    // }

    public override void OnBodyGUI() {
        _baseNode = target as BaseNode;
        serializedObject.Update();
        if(_baseNode.Running()){
            UnityEditor.EditorGUI.ProgressBar(new Rect(0,0, GetWidth(), 10), _baseNode.GetTickProgress(),"");
        }


        serializedObject.ApplyModifiedProperties();
        base.OnBodyGUI();
    }
    // public override int GetWidth()
    // {
    //     return 300;
    // }

}
