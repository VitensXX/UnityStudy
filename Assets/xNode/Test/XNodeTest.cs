using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class XNodeTest : MonoBehaviour
{
    public SimpleGraph simpleGraph;
    
    private void Start() {
        List<Node> nodes = simpleGraph.nodes;
        for (int i = 0; i < nodes.Count; i++)
        {
            SimpleNode simpleNode = nodes[i] as SimpleNode;
            // if(simpleNode.GetPort("a").GetInputValue() == null){
            //     Debug.LogError(simpleNode.GetSum());
            // }
            // else{
            //     simpleNode.a = (int)simpleNode.GetPort("a").GetInputValue();
            //     Debug.LogError(simpleNode.GetSum());
            // }
            // Debug.LogError(simpleNode.GetPort("a").GetInputValue());
            // simpleNode.a = (int)simpleNode.GetValue(simpleNode.GetPort("sum"));
            // Debug.LogError(simpleNode.a);
            // Debug.LogError(simpleNode.name + " " +simpleNode.GetValue(simpleNode.GetPort("a")));

            if(simpleNode.GetNext() != null){
                Debug.LogError(simpleNode.GetNext().desc);
            }
            else{
                Debug.LogError("No next");
            }
        }
    }
}
