using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class SkillRun : MonoBehaviour
{
    public SkillGraph skill;

    private void Start() {
        List<Node> nodes = skill.nodes;
        HeadNode head = null;
        for (int i = 0; i < nodes.Count; i++)
        {
            //找到头结点
            if(typeof(HeadNode) == nodes[i].GetType()){
                head = nodes[i] as HeadNode;
                break;
            }
        }
        
        Debug.LogError(head.skillName+"开始执行");
        // BranchNode branchNode = head.Next() as BranchNode;
        // Debug.LogError(branchNode.branchType);

        // Node next = branchNode.GetNextNode();
        // if(next)

        BaseNode next = head.Next();
        while(next != null){
            next.Run(()=>{
                next = next.Next();
            });
        }

        Debug.LogError("END");
    }

    // BaseNode Next(BaseNode curNode){
    //     BaseNode next = curNode.Next();
    //     if(next != null){
    //         return next;
    //     }
    //     else{

    //     }
    // }
}
