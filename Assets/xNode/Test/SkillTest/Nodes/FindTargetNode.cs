using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class FindTargetNode : BaseNode
{
    [Input]public string input;
    public string FindTarget;

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

    public override void Start()
    {
        Debug.LogError(FindTarget);
    }

}
