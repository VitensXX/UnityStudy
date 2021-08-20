using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class XNodeTest : MonoBehaviour
{
    public SimpleGraph simpleGraph;
    
    private void Start() {
        List<Node> nodes = simpleGraph.nodes;
        // for (int i = 0; i < nodes.Count; i++)
        // {
        //     SimpleNode simpleNode = nodes[i] as SimpleNode;
        //     // if(simpleNode.GetPort("a").GetInputValue() == null){
        //     //     Debug.LogError(simpleNode.GetSum());
        //     // }
        //     // else{
        //     //     simpleNode.a = (int)simpleNode.GetPort("a").GetInputValue();
        //     //     Debug.LogError(simpleNode.GetSum());
        //     // }
        //     // Debug.LogError(simpleNode.GetPort("a").GetInputValue());
        //     // simpleNode.a = (int)simpleNode.GetValue(simpleNode.GetPort("sum"));
        //     // Debug.LogError(simpleNode.a);
        //     // Debug.LogError(simpleNode.name + " " +simpleNode.GetValue(simpleNode.GetPort("a")));

        //     if(simpleNode.GetSumNext() != null){
        //         Debug.LogError(simpleNode.GetSumNext().desc);
        //     }
        //     else{
        //         Debug.LogError("No next");
        //     }
        // }\
        
        SimpleNode simpleNode = nodes[0] as SimpleNode;
        SimpleNode subTest = simpleNode.GetSubNext();
        subTest.a = (int)subTest.GetPort("a").GetInputValue();
        TestDebug(subTest);
        SimpleNode SSSUm = subTest.GetSumNext();
        SSSUm.a = (int)SSSUm.GetPort("a").GetInputValue();
        TestDebug(SSSUm);
        
        SimpleNode SSSUb = subTest.GetSubNext();
        SSSUb.a = (int)SSSUb.GetPort("a").GetInputValue();
        TestDebug(SSSUb);
    }

    void TestDebug(SimpleNode node){
        Debug.LogError(node.desc+ " sum:"+node.GetSum()+"  sub:"+node.GetSub());
    }
}
