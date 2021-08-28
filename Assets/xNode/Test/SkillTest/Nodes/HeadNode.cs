using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;


public class HeadNode : BaseNode
{
    public string Id;
    public string skillName;
    [Output]public string branchOutput;
    
    public override object GetValue(NodePort port){
        return "";
    }

    public override BaseNode Next(){
        NodePort nodePort = GetPort("branchOutput");
        if(nodePort.Connection == null){
            return null;
        }
        else{
            return nodePort.Connection.node as BaseNode;
        }
    }

    public override void Start()
    {
        Debug.LogError("head");
    }

}
