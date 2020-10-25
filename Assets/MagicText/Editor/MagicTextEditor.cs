using UnityEngine;
using System.Collections;
using UnityEditor;


//支持中文.
/*----------------------------------------------------------------
// Copyright (C) 公司名称 成都微美互动科技有限公司
// 版权所有。  
//
// 文件名：MagicTextEditor.cs
// 文件功能描述：
 
// 创建标识：Created by Vitens On 2019/08/16 20:27:26

// 修改标识：
// 修改描述：
//----------------------------------------------------------------*/

[CustomEditor(typeof(MagicText))]
public class MagicTextEditor : Editor
{
    SerializedObject so;
    SerializedProperty _type, _frequency, _perTextInterval, _loopInterval, _perTextSpeed, _periodicFunctionType,
        _animFactor, _scale, _textColor, _noiseTex, _intensity, _interval, _atten, _playOnAwake,
        _loop, _isFadein, _animCurve, _alphaCurve, _isRandomForLoopInterval, _randomRangeForLoopInterval,
        _openPosOffset, _posOffset,_randomOrder, _radius, _textInterval, _textIntervalFactor;
    MagicText _ctrl;

    protected void OnEnable()
    {
        so = new SerializedObject(target);
        _type = so.FindProperty("type");
        _frequency = so.FindProperty("frequency");
        _perTextInterval = so.FindProperty("perTextInterval");
        _loopInterval = so.FindProperty("loopInterval");
        _animFactor = so.FindProperty("animFactor");
        _scale = so.FindProperty("scale");
        _textColor = so.FindProperty("textColor");
        _interval = so.FindProperty("interval");
        _perTextSpeed = so.FindProperty("perTextSpeed");
        _periodicFunctionType = so.FindProperty("periodicType");
        _atten = so.FindProperty("atten");
        _playOnAwake = so.FindProperty("playOnAwake");
        _loop = so.FindProperty("Loop");
        _isFadein = so.FindProperty("isFadein");
        _animCurve = so.FindProperty("animCurve");
        _alphaCurve = so.FindProperty("alphaCurve");
        _isRandomForLoopInterval = so.FindProperty("isRandomForLoopInterval");
        _randomRangeForLoopInterval = so.FindProperty("randomRangeForLoopInterval");
        _openPosOffset = so.FindProperty("openPosOffset");
        _posOffset = so.FindProperty("posOffset");
        _randomOrder = so.FindProperty("randomOrder");
        _radius = so.FindProperty("radius");
        _textInterval = so.FindProperty("textInterval");
        _textIntervalFactor = so.FindProperty("textIntervalFactor");
        _ctrl = target as MagicText;
    }

    public override void OnInspectorGUI()
    {
        so.Update();

        EditorGUI.BeginChangeCheck();
       
        EditorGUILayout.PropertyField(_playOnAwake);
        EditorGUILayout.PropertyField(_textInterval);
        EditorGUILayout.PropertyField(_textIntervalFactor);
        EditorGUILayout.PropertyField(_loop);

        using (new BoxScope(false))
        {
            if (_ctrl.Loop)
            {
                EditorGUILayout.LabelField("loop相关参数设置");
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_loopInterval);
                EditorGUILayout.PropertyField(_isRandomForLoopInterval);
                if (_ctrl.isRandomForLoopInterval)
                {
                    EditorGUILayout.PropertyField(_randomRangeForLoopInterval);
                }
                EditorGUI.indentLevel--;
            }
            else
            {
                EditorGUILayout.LabelField("勾选才能支持渐入效果.");
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_isFadein);
                if (_ctrl.isFadein)
                {
                    EditorGUILayout.PropertyField(_alphaCurve);
                }
                EditorGUI.indentLevel--;
            }
        }
        using (new BoxScope(false))
        {
            EditorGUILayout.LabelField("动画参数设置");
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_type);
            EditorGUILayout.PropertyField(_animCurve);
            EditorGUILayout.PropertyField(_randomOrder);
            switch (_ctrl.type)
            {
                case MagicText.Type.Normal:
                    ShowNormalEditor();
                    break;

                case MagicText.Type.Jump:
                    ShowJumpEditor();
                    break;
                case MagicText.Type.Stretch:
                    ShowJumpEditor();
                    break;

                case MagicText.Type.Rotate:
                    ShowRotateEditor();
                    break;
                case MagicText.Type.CircleLayout:
                    ShowRotateEditor();
                    EditorGUILayout.PropertyField(_radius);
                    break;
                case MagicText.Type.Scale:
                    ShowScaleEditor();
                    break;

                case MagicText.Type.Color:
                    ShowColorEditor();
                    break;

                case MagicText.Type.Rainbow:
                    ShowRainbowEditor();
                    break;
            }

            if (!_ctrl.Loop)
            {
                EditorGUILayout.PropertyField(_openPosOffset);
            }
            if (_ctrl.openPosOffset)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_posOffset);
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.BeginHorizontal();

       
        if (_ctrl.IsStart)
        {
            GUI.color = Color.yellow;
            if (GUILayout.Button("Pause"))
            {
                _ctrl.Pause();
            }
        }
        else
        {
            GUI.color = Color.green;
            if (GUILayout.Button("Play "))
            {
                _ctrl.Play();
            }
        }

        GUI.color = Color.red;
        if (GUILayout.Button("Stop "))
        {
            _ctrl.Stop();
        }
        GUI.color = Color.white;
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Replay"))
        {
            _ctrl.Replay();
        }
        if (!_ctrl.Loop)
        {
            if (GUILayout.Button("Fadeout"))
            {
                _ctrl.Fadeout();
            }
        }
        
        EditorGUILayout.EndHorizontal();
        so.ApplyModifiedProperties();
    }

    void ShowCommonEditor()
    {
        //using (new BoxScope(false))
        //{
            //GUILayout.Label("通用参数:");
            //EditorGUI.indentLevel++;
            //EditorGUILayout.PropertyField(_periodicFunctionType);
            //EditorGUILayout.PropertyField(_frequency);
            EditorGUILayout.PropertyField(_perTextSpeed);
            EditorGUILayout.PropertyField(_perTextInterval);

        //if (_ctrl.Loop)
        //{
        //    EditorGUILayout.PropertyField(_loopInterval);
        //    EditorGUILayout.PropertyField(_isRandomForLoopInterval);
        //    EditorGUI.indentLevel++;
        //    if (_ctrl.isRandomForLoopInterval)
        //    {
        //        EditorGUILayout.PropertyField(_randomRangeForLoopInterval);
        //    }
        //    EditorGUI.indentLevel--;
        //}
            //EditorGUI.indentLevel--;
        //}
    }

    void ShowNormalEditor()
    {
        ShowCommonEditor();
    }

    void ShowJumpEditor()
    {
        EditorGUILayout.PropertyField(_animFactor);
        ShowCommonEditor();
    }

    void ShowColorEditor()
    {
        EditorGUILayout.PropertyField(_textColor);
        ShowCommonEditor();
    }

    void ShowRainbowEditor()
    {
        EditorGUILayout.PropertyField(_perTextSpeed);
        EditorGUILayout.PropertyField(_perTextInterval);
        EditorGUILayout.PropertyField(_loopInterval);
    }

    void ShowRotateEditor()
    {
        ShowCommonEditor();
    }

    void ShowScaleEditor()
    {
        EditorGUILayout.PropertyField(_animFactor);
        ShowCommonEditor();
    }
}
