using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class BranchNode : BaseNode
{
    [Input]public string BranchInput;
    public BranchType branchType;

    [Output]public string branchTrue;
    [Output]public string branchFalse;

    public override object GetValue(NodePort port){
        return "";
    }
    bool select = false;
    public override BaseNode Next(){
        if(select){
            NodePort nodePort = GetPort("branchTrue");
            if(nodePort.Connection == null){
                return null;
            }
            else{
                return nodePort.Connection.node as BaseNode;
            }
        }
        else{
            NodePort nodePort = GetPort("branchFalse");
            if(nodePort.Connection == null){
                return null;
            }
            else{
                return nodePort.Connection.node as BaseNode;
            }
        }
    }

    public override void Run(System.Action finished)
    {
        int random = Random.Range(0, 10);
        Debug.LogError("分支执行，随机:"+random);
        select = random < 5;
    }
}
