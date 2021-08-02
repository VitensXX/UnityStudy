using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Created by Vitens on 2021/7/31 15:17:54
/// 
/// Description : 
///     自定义EditorWindow基类
/// </summary>
public class VitensEditorWindow : EditorWindow
{
    protected List<Vector2> vector2s;
    protected List<string> strs;
    protected List<bool> bools;
    protected List<Color> colors;

#region 申请需要的属性 这样在基类中管理属性，子类有需要直接申请就能使用
    
    //申请Vector2属性
    protected void RequestVector2s(int count){
        if(vector2s == null){
            vector2s = new List<Vector2>();
        }

        for (int i = 0; i < count; i++)
        {
            vector2s.Add(new Vector2());
        }
    }

    protected void RequestStrings(int count){
        if(strs == null){
            strs = new List<string>();
        }

        for (int i = 0; i < count; i++)
        {
            strs.Add(string.Empty);
        }
    }

    protected void RequestBools(int count){
        if(bools == null){
            bools = new List<bool>();
        }

        for (int i = 0; i < count; i++)
        {
            bools.Add(false);
        }
    }

    protected void RequestColors(int count){
        if(colors == null){
            colors = new List<Color>();
        }

        for (int i = 0; i < count; i++)
        {
            colors.Add(Color.white);
        }
    }
    
#endregion 申请需要的属性 这样在基类中管理属性，子类有需要直接申请就能使用

    

    //重新初始化
    protected virtual void Init(){
        Clear();
    }

    //重新聚焦到自己窗口 通过聚焦其他窗口模拟丢失焦点的操作 然后再重新聚焦回来
    protected void ReFocus(){
        var consoleWindowType = typeof(EditorWindow).Assembly.GetType("UnityEditor.ConsoleWindow");
        GetWindow(consoleWindowType).Focus();
        Focus();
    }

    //清除操作
    void Clear(){
        if(vector2s != null){
            vector2s.Clear();
        }
    }

    private void OnDestroy() {
        Clear();
    }
}
