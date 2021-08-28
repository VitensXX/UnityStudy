using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public abstract class BaseNode : Node
{

    public abstract BaseNode Next();

    //节点开始执行方法,子类继承一定要调用Base
    public virtual void Start(){
        _tick = 0;
    }

    //节点结束执行方法,子类继承一定要调用Base
    public virtual void End(){
        _tick = -1;
    }
    
    public virtual float GetDuration(){
        return 0;
    }

    #region 计时相关,为编辑器服务的
    private float _tick = -1;//负一表示还未执行到
    public void RefreshTick(float tick){
        _tick = tick;
    }

    public float GetTickProgress(){
        float duration = GetDuration();
        if(duration <= 0.001f){
            return 1;
        }
        else{
            return _tick / duration;
        }
    }

    public bool Running(){
        return _tick >= 0;
    }


        
    #endregion
}
