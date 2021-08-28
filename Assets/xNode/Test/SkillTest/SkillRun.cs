using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class SkillRun : MonoBehaviour
{
    public SkillGraph skill;

    BaseNode _curNode;
    private void Start() {
        Init();
    }

    void Init(){
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
        _curNode = head;
        Play();
    }

    void Play(){
        _running = true;
    }

    void Stop(){
        _running = false;
    }

    bool _running;
    float _tick = 0;
    private void Update() {
        if(!_running){
            return;
        }

        _curNode.RefreshTick(_tick);
        if(_tick >= _curNode.GetDuration()){
            Next();
        }
        _tick += Time.deltaTime;

    }

    void Next(){
        _tick = 0;
        _curNode.End();
        _curNode = _curNode.Next();
        if(_curNode == null){
            Stop();
        }
        else{
            _curNode.Start();
        }
    }

    void Restart(){
        Init();
    }

    private void OnGUI() {
        if(GUILayout.Button("Restart")){
            Restart();
        }
    }

}
