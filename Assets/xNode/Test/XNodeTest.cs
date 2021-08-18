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
            // Debug.LogError(simpleNode.name + " " +simpleNode.GetSum());
        }

    }
}
