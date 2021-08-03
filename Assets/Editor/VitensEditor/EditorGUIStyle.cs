using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

/// <summary>
/// Created by Vitens on 2021/7/31 1:45:49
/// 
/// Description : 
///     一些常用的GUIStyle样式
/// </summary>
public class EditorGUIStyle
{
    public static GUIStyle Box = "box";
    public static GUIStyle SearchTextField = "SearchTextField";
    public static GUIStyle SearchCancelButton = "SearchCancelButton";
    public static GUIStyle SearchCancelButtonEmpty = "SearchCancelButtonEmpty";
    public static GUIStyle OL_EntryBackEven = "OL EntryBackEven";
    public static GUIStyle OL_EntryBackOdd = "OL EntryBackOdd";
    public static GUIStyle OL_Pluse = "OL Plus";
    public static GUIStyle OL_Minus = "OL Minus";
    public static GUIStyle CNStatusInfo = "CN StatusInfo";
    public static GUIStyle CNStatusWarn = "CN StatusWarn";
    public static GUIStyle CNStatusError = "CN StatusError";
    public static GUIStyle AM_HeaderStyle = "AM HeaderStyle";
    public static GUIStyle AM_MixerHeader = "AM MixerHeader";
    public static GUIStyle AM_MixerHeader2 = "AM MixerHeader2";
}

// public class BoxScope : System.IDisposable
// {
//     readonly bool indent;

//     static GUIStyle boxScopeStyle;
//     public static GUIStyle BoxScopeStyle
//     {
//         get
//         {
//             if (boxScopeStyle == null)
//             {
//                 boxScopeStyle = new GUIStyle(EditorStyles.helpBox);
//                 RectOffset p = boxScopeStyle.padding;                  
//                 p.right += 6;
//                 p.top += 1;
//                 p.left += 3;
//             }

//             return boxScopeStyle;
//         }
//     }

//     public BoxScope(bool indent = true)
//     {
//         this.indent = indent;
//         EditorGUILayout.BeginVertical(BoxScopeStyle);
//         if (indent) EditorGUI.indentLevel++;
//     }

//     public void Dispose()
//     {
//         if (indent) EditorGUI.indentLevel--;
//         EditorGUILayout.EndVertical();
//     }
// }

public class ModifyGUIContetnColor : IDisposable
{
    Color _originContentColor;

    public ModifyGUIContetnColor(bool modify, Color color)
    {
        _originContentColor = GUI.contentColor;
        if(modify)
            GUI.contentColor = color;
    }

    public void Dispose()
    {
        GUI.contentColor = _originContentColor;
    }
}

public class ModifyGUIBackGroundColor : IDisposable
{
    Color _originBackGroundColor;

    public ModifyGUIBackGroundColor(bool modify, Color color)
    {
        _originBackGroundColor = GUI.backgroundColor;
        if (modify)
            GUI.backgroundColor = color;
    }

    public void Dispose()
    {
        GUI.backgroundColor = _originBackGroundColor;
    }
}

public class DisableGroup : IDisposable
{
    public DisableGroup(bool disable)
    {
        EditorGUI.BeginDisabledGroup(disable);
    }

    public void Dispose()
    {
        EditorGUI.EndDisabledGroup();
    }
}

