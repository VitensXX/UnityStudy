using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class ActionNode : BaseNode
{
    [Input]public string ActionInput;
    public string action;
    public float duration;
    [Output]public string next;

    public override BaseNode Next(){
        NodePort nodePort = GetPort("next");
        if(nodePort.Connection == null){
            return null;
        }
        else{
            return nodePort.Connection.node as BaseNode;
        }
    }

    public override void Run(System.Action finished)
    {
        Debug.LogError("播放动作：" + action+"  duration:"+duration);

        finished?.Invoke();
    }
}
