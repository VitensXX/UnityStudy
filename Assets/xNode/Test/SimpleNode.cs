using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using System;
public class SimpleNode : Node{
    [Input] public int a;
    public int b;
    public string desc;
    public float duration;
    [Output] public int sum;


    //获取对应端口的值    
    public override object GetValue(NodePort port) {
        return GetSum();
    }

    public int GetSum() {
        return a + b;
    }

    public SimpleNode GetNext(){
        NodePort nodePort = GetPort("sum");
        if(nodePort.Connection == null){
            return null;
        }
        else{
            return nodePort.Connection.node as SimpleNode;
        }
    }

    public void Play(Action finished){
        
    }
}