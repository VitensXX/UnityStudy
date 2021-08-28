using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class ActionNode : BaseNode
{
    [Input]public string input;
    public string action;
    public float duration;
    public GameObject timeline;
    [Output]public string output;

    public override BaseNode Next(){
        NodePort nodePort = GetPort("output");
        if(nodePort.Connection == null){
            return null;
        }
        else{
            return nodePort.Connection.node as BaseNode;
        }
    }

    GameObject _test;
    public override void Start()
    {
        Debug.LogError("播放动作：" + action+"  duration:"+duration);
        _test = GameObject.Instantiate<GameObject>(timeline);
        _test.name = action +" "+duration;
    }

    public override void End()
    {
        base.End();
        GameObject.Destroy(_test);
    }



    public override float GetDuration()
    {
        return duration;
    }
}
