using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class EndNode : BaseNode
{
    [Input]public string endInput;
    public override BaseNode Next(){
        return null;
    }

    public override void Run(System.Action finished)
    {
        Debug.LogError("结束节点了");
        finished?.Invoke();
    }
}
