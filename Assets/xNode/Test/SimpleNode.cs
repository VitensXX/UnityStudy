using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
public class SimpleNode : Node{
    [Input] public int a;
    [Output]public int b;
    [Output] public int sum;


    //获取对应端口的值    
    public override object GetValue(NodePort port) {
        return GetSum();
    }

    public int GetSum() {
        return a + b;
    }
}