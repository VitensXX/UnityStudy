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
    [Output] public int sub;


    //获取对应端口的值    
    public override object GetValue(NodePort port) {
        if(port.fieldName == "sum"){
            return GetSum();
        }
        else{
            return GetSub();
        }
    }

    public int GetSum() {
        return a + b;
    }

    public int GetSub(){
        return a - b;
    }

    public SimpleNode GetSumNext(){
        NodePort nodePort = GetPort("sum");
        if(nodePort.Connection == null){
            return null;
        }
        else{
            return nodePort.Connection.node as SimpleNode;
        }
    }

    public SimpleNode GetSubNext(){
        NodePort nodePort = GetPort("sub");
        if(nodePort.Connection == null){
            return null;
        }
        else{
            return nodePort.Connection.node as SimpleNode;
        }
    }

    public void Play(Action finished){
        Debug.LogError("test");
    }
}