using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChange : MonoBehaviour
{
    public GameObject scene1;
    public GameObject scene2;
    public float forwardDuration;
    public float backDuration;
    public float sceneLoadedCost;

    float _tick;
    bool _forward = true;
    bool _start = false;
    bool _loading = false;

    PostProcessSceneChange _post;

    public float tick => _tick;

    // Start is called before the first frame update
    void Start()
    {
        _post = GetComponent<PostProcessSceneChange>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!_start){
            return;
        }
        float deltaTime = Time.deltaTime;

        if(_loading){
            _tick += deltaTime;
            if(_tick >= sceneLoadedCost){
                _loading = false;
                _forward = false;
                _tick = 0;
                DoSceneChange();
            }
        }
        else{
            _tick += deltaTime;
            if(_tick >= forwardDuration && _forward){
                _tick = 0;
                _loading = true;
            }

            if(_tick >= backDuration && !_forward){
                _tick = 1;
                _start = false;
                _post.factor = 0;
                return;
            }
        }

        if(!_loading){
            if(_forward){
                _post.factor = _tick / forwardDuration;
            }
            else{
                _post.factor = 1 - _tick / backDuration;
            }
        }
    }

    bool _to = true;

    public void RePlay(){
        _start = true;
        _forward = true;
        _tick = 0;
        _loading = false;
        _to = !_to;
    }

    void DoSceneChange(){
        scene1.SetActive(_to);
        scene2.SetActive(!_to);
    }
}
