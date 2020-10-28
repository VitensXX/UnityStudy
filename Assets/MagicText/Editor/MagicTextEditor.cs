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

    SerializedProperty _enableAnim, _unscaledTime, _playOnAwake, _radius, _angle, _italicFactor, _awakeDelay,
        _loop_0, _loopInterval_0, _isRandomForLoopInterval_0, _randomRangeForLoopInterval_0;
    //布局相关参数
    SerializedProperty _layout;

    SerializedProperty _type_1, _perTextInterval_1, _loopInterval_1, _perTextSpeed_1,_animFactor_1,
        _textColor_1, _loop_1, _isFadein_1, _animCurve_1, _alphaCurve_1, _isRandomForLoopInterval_1, _randomRangeForLoopInterval_1,
        _openPosOffset_1, _posOffset_1,_randomOrder_1;

    SerializedProperty _stage_2, _type_2, _perTextInterval_2, _loopInterval_2, _perTextSpeed_2, _animFactor_2,
        _textColor_2, _loop_2, _animCurve_2, _isRandomForLoopInterval_2, _randomRangeForLoopInterval_2,
        _openPosOffset_2, _posOffset_2, _randomOrder_2, _waitForStageSwitch_2;

    SerializedProperty _stage_3, _type_3, _perTextInterval_3, _loopInterval_3, _perTextSpeed_3, _animFactor_3,
        _textColor_3, _loop_3, _animCurve_3, _isRandomForLoopInterval_3, _randomRangeForLoopInterval_3,
        _openPosOffset_3, _posOffset_3, _randomOrder_3, _waitForStageSwitch_3, _forceFadeoutDelay_3, _forceFadeoutTime_3;

    MagicText _ctrl;

    protected void OnEnable()
    {
        so = new SerializedObject(target);


        _enableAnim = so.FindProperty("enableAnim");
        _unscaledTime = so.FindProperty("unscaledTime");
        _playOnAwake = so.FindProperty("playOnAwake");
        _awakeDelay = so.FindProperty("awakeDelay");
        _loop_0 = so.FindProperty("loop_0");
        _loopInterval_0 = so.FindProperty("loopInterval_0");
        _isRandomForLoopInterval_0 = so.FindProperty("isRandomForLoopInterval_0");
        _randomRangeForLoopInterval_0 = so.FindProperty("randomRangeForLoopInterval_0");


        //layout
        _layout = so.FindProperty("layout");
        _ctrl = target as MagicText;
        _italicFactor = so.FindProperty("italicFactor");
        _radius = so.FindProperty("radius");
        _angle = so.FindProperty("angle");

        //fadein  第一阶段
        _type_1 = so.FindProperty("type_1");
        _perTextInterval_1 = so.FindProperty("perTextInterval_1");
        _loopInterval_1 = so.FindProperty("loopInterval_1");
        _animFactor_1 = so.FindProperty("animFactor_1");
        _textColor_1 = so.FindProperty("textColor_1");
        _perTextSpeed_1 = so.FindProperty("perTextSpeed_1");
        _loop_1 = so.FindProperty("loop_1");
        _isFadein_1 = so.FindProperty("fadein_1");
        _animCurve_1 = so.FindProperty("animCurve_1");
        _alphaCurve_1 = so.FindProperty("alphaCurve_1");
        _isRandomForLoopInterval_1 = so.FindProperty("isRandomForLoopInterval_1");
        _randomRangeForLoopInterval_1 = so.FindProperty("randomRangeForLoopInterval_1");
        _openPosOffset_1 = so.FindProperty("openPosOffset_1");
        _posOffset_1 = so.FindProperty("posOffset_1");
        _randomOrder_1 = so.FindProperty("randomOrder_1");

        //display 第二阶段
        _stage_2 = so.FindProperty("stage_2");
        _type_2 = so.FindProperty("type_2");
        _perTextInterval_2 = so.FindProperty("perTextInterval_2");
        _loopInterval_2 = so.FindProperty("loopInterval_2");
        _animFactor_2 = so.FindProperty("animFactor_2");
        _textColor_2 = so.FindProperty("textColor_2");
        _perTextSpeed_2 = so.FindProperty("perTextSpeed_2");
        _loop_2 = so.FindProperty("loop_2");
        _animCurve_2 = so.FindProperty("animCurve_2");
        _isRandomForLoopInterval_2 = so.FindProperty("isRandomForLoopInterval_2");
        _randomRangeForLoopInterval_2 = so.FindProperty("randomRangeForLoopInterval_2");
        _openPosOffset_2 = so.FindProperty("openPosOffset_2");
        _posOffset_2 = so.FindProperty("posOffset_2");
        _randomOrder_2 = so.FindProperty("randomOrder_2");
        _waitForStageSwitch_2 = so.FindProperty("waitForStageSwitch_2");

        //fadeout 第三阶段
        _stage_3 = so.FindProperty("stage_3");
        _type_3 = so.FindProperty("type_3");
        _perTextInterval_3 = so.FindProperty("perTextInterval_3");
        _loopInterval_3 = so.FindProperty("loopInterval_3");
        _animFactor_3 = so.FindProperty("animFactor_3");
        _textColor_3 = so.FindProperty("textColor_3");
        _perTextSpeed_3 = so.FindProperty("perTextSpeed_3");
        _loop_3 = so.FindProperty("loop_3");
        _animCurve_3 = so.FindProperty("animCurve_3");
        _isRandomForLoopInterval_3 = so.FindProperty("isRandomForLoopInterval_3");
        _randomRangeForLoopInterval_3 = so.FindProperty("randomRangeForLoopInterval_3");
        _openPosOffset_3 = so.FindProperty("openPosOffset_3");
        _posOffset_3 = so.FindProperty("posOffset_3");
        _randomOrder_3 = so.FindProperty("randomOrder_3");
        _waitForStageSwitch_3 = so.FindProperty("waitForStageSwitch_3");
        _forceFadeoutDelay_3 = so.FindProperty("forceFadeoutDelay_3");
        _forceFadeoutTime_3 = so.FindProperty("forceFadeoutTime_3");
    }

    bool _fadeinFold, _displayFold, _fadeoutFold;
    public override void OnInspectorGUI()
    {
        so.Update();

        EditorGUI.BeginChangeCheck();

        //布局设置
        LayoutInspector();

        EditorGUILayout.PropertyField(_enableAnim);
        if (_ctrl.enableAnim)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_unscaledTime);
            EditorGUILayout.PropertyField(_playOnAwake);
            //需要fadein才由awakeDelay控制最初的透明不显示节奏 否则由display阶段的wait控制
            if (_ctrl.playOnAwake && _ctrl.fadein_1)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_awakeDelay);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.PropertyField(_loop_0);

            if (_ctrl.loop_0)
            {
                using (new BoxScope(false))
                {
                    EditorGUILayout.LabelField("所有阶段为一个整体的loop设置");
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(_loopInterval_0);
                    EditorGUILayout.PropertyField(_isRandomForLoopInterval_0);
                    if (_ctrl.isRandomForLoopInterval_0)
                    {
                        EditorGUILayout.PropertyField(_randomRangeForLoopInterval_0);
                    }
                    EditorGUI.indentLevel--;
                }
            }

            EditorGUI.indentLevel--;

            EditorGUILayout.Space();
            //第一阶段 淡入
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(_isFadein_1, new GUIContent("第一阶段【淡入】"));
            if (_ctrl.fadein_1 && _ctrl.loop_1)
            {
                EditorGUILayout.LabelField("loop");
            }
            EditorGUILayout.EndHorizontal();
            if (_ctrl.fadein_1)
            {
                using (new BoxScope(true))
                {
                    _fadeinFold = EditorGUILayout.Foldout(_fadeinFold, "淡入参数设置", true);
                    if (_fadeinFold)
                    {
                        FadeinInspector();
                    }
                }
            }

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(_stage_2, new GUIContent("第二阶段【持续展示阶段】"));
            if(_ctrl.stage_2 && _ctrl.loop_2)
            {
                EditorGUILayout.LabelField("loop");
            }
            EditorGUILayout.EndHorizontal();
            if (_ctrl.stage_2)
            {
                using (new BoxScope(true))
                {
                    _displayFold = EditorGUILayout.Foldout(_displayFold, "展示阶段参数设置", true);
                    if (_displayFold)
                    {
                        DisplayStageInspector();
                    }
                }
            }

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(_stage_3, new GUIContent("第三阶段【淡出阶段】"));
            if (_ctrl.stage_3 )
            {
                string tip = "";
                if (_ctrl.loop_3)
                    tip = "loop   ";
                if (_ctrl.forceFadeoutDelay_3)
                    tip += "ForceFadeout delay:" + _ctrl.forceFadeoutTime_3;
                EditorGUILayout.LabelField(tip);
            }
            EditorGUILayout.EndHorizontal();
            if (_ctrl.stage_3)
            {
                using (new BoxScope(true))
                {
                    _fadeoutFold = EditorGUILayout.Foldout(_fadeoutFold, "淡出阶段参数设置", true);
                    if (_fadeoutFold)
                    {
                        FadeoutInspector();
                    }
                }
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
            if (!_ctrl.loop_1)
            {
                if (GUILayout.Button("Fadeout"))
                {
                    _ctrl.Fadeout();
                }
            }

            EditorGUILayout.EndHorizontal();
        }
        so.ApplyModifiedProperties();
    }

    bool _layoutFold;
    void LayoutInspector()
    {
        using (new BoxScope(true))
        {
            _layoutFold = EditorGUILayout.Foldout(_layoutFold, "布局设置", true);
            if (_layoutFold)
            {
                EditorGUILayout.PropertyField(_layout);

                if(_ctrl.layout == MagicText.Layout.Circle)
                {
                    EditorGUILayout.PropertyField(_angle);
                    EditorGUILayout.PropertyField(_radius);
                }

            }
        }
    }

    //淡入设置界面
    void FadeinInspector()
    {
        EditorGUILayout.PropertyField(_alphaCurve_1);
        EditorGUILayout.PropertyField(_loop_1);

        if (_ctrl.loop_1)
        {
            using (new BoxScope(false))
            {
                EditorGUILayout.LabelField("loop相关参数设置");
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_loopInterval_1);
                EditorGUILayout.PropertyField(_isRandomForLoopInterval_1);
                if (_ctrl.isRandomForLoopInterval_1)
                {
                    EditorGUILayout.PropertyField(_randomRangeForLoopInterval_1);
                }
                EditorGUI.indentLevel--;
            }
        }

        using (new BoxScope(false))
        {
            EditorGUILayout.LabelField("动画参数设置");
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_type_1, new GUIContent("Type"));
            EditorGUILayout.PropertyField(_animCurve_1, new GUIContent("Anim Curve"));
            if (!_ctrl.CheckCurve())
            {
                EditorGUILayout.HelpBox("Anim Curve曲线横坐标区间应该限制在[0-1]之间", MessageType.Error);
            }

            if(_ctrl.type_1 == MagicText.Type.Color)
            {
                EditorGUILayout.PropertyField(_textColor_1, new GUIContent("Color"));
            }
            else if(_ctrl.type_1 != MagicText.Type.Normal)
            {
                EditorGUILayout.PropertyField(_animFactor_1, new GUIContent("Anim Factor"));
            }

            EditorGUILayout.PropertyField(_perTextSpeed_1);
            EditorGUILayout.PropertyField(_perTextInterval_1);
            EditorGUILayout.PropertyField(_randomOrder_1);
            EditorGUILayout.PropertyField(_openPosOffset_1);
            if (_ctrl.openPosOffset_1)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_posOffset_1);
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
        }
    }

    void DisplayStageInspector()
    {
        EditorGUILayout.PropertyField(_waitForStageSwitch_2);
        EditorGUILayout.PropertyField(_loop_2);

        if (_ctrl.loop_2)
        {
            using (new BoxScope(false))
            {
                EditorGUILayout.LabelField("loop相关参数设置");
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_loopInterval_2);
                EditorGUILayout.PropertyField(_isRandomForLoopInterval_2);
                if (_ctrl.isRandomForLoopInterval_2)
                {
                    EditorGUILayout.PropertyField(_randomRangeForLoopInterval_2);
                }
                EditorGUI.indentLevel--;
            }
        }

        using (new BoxScope(false))
        {
            EditorGUILayout.LabelField("动画参数设置");
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_type_2, new GUIContent("Type"));
            EditorGUILayout.PropertyField(_animCurve_2, new GUIContent("Anim Curve"));
            if (!_ctrl.CheckCurve())
            {
                EditorGUILayout.HelpBox("Anim Curve曲线横坐标区间应该限制在[0-2]之间", MessageType.Error);
            }

            if (_ctrl.type_2 == MagicText.Type.Color)
            {
                EditorGUILayout.PropertyField(_textColor_2, new GUIContent("Color"));
            }
            else if (_ctrl.type_2 != MagicText.Type.Normal)
            {
                EditorGUILayout.PropertyField(_animFactor_2, new GUIContent("Anim Factor"));
            }

            EditorGUILayout.PropertyField(_perTextSpeed_2);
            EditorGUILayout.PropertyField(_perTextInterval_2);
            EditorGUILayout.PropertyField(_randomOrder_2);
            EditorGUILayout.PropertyField(_openPosOffset_2);
            if (_ctrl.openPosOffset_2)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_posOffset_2);
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
        }
    }

    void FadeoutInspector()
    {
        EditorGUILayout.PropertyField(_forceFadeoutDelay_3);
        if (_ctrl.forceFadeoutDelay_3)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_forceFadeoutTime_3);
            EditorGUI.indentLevel--;
        }
        else
        {
            EditorGUILayout.PropertyField(_waitForStageSwitch_3);
        }
        EditorGUILayout.PropertyField(_loop_3);

        if (_ctrl.loop_3)
        {
            using (new BoxScope(false))
            {
                EditorGUILayout.LabelField("loop相关参数设置");
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_loopInterval_3);
                EditorGUILayout.PropertyField(_isRandomForLoopInterval_3);
                if (_ctrl.isRandomForLoopInterval_3)
                {
                    EditorGUILayout.PropertyField(_randomRangeForLoopInterval_3);
                }
                EditorGUI.indentLevel--;
            }
        }

        using (new BoxScope(false))
        {
            EditorGUILayout.LabelField("动画参数设置");
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_type_3, new GUIContent("Type"));
            EditorGUILayout.PropertyField(_animCurve_3, new GUIContent("Anim Curve"));
            if (!_ctrl.CheckCurve())
            {
                EditorGUILayout.HelpBox("Anim Curve曲线横坐标区间应该限制在[0-3]之间", MessageType.Error);
            }

            //if(_ctrl.type_3 == MagicText.FadeoutType.Other)
            //{
                if (_ctrl.type_3 == MagicText.Type.Color)
                {
                    EditorGUILayout.PropertyField(_textColor_3, new GUIContent("Color"));
                }
                else if (_ctrl.type_3 != MagicText.Type.Normal)
                {
                    EditorGUILayout.PropertyField(_animFactor_3, new GUIContent("Anim Factor"));
                }

            //}

            EditorGUILayout.PropertyField(_perTextSpeed_3);
            EditorGUILayout.PropertyField(_perTextInterval_3);
            EditorGUILayout.PropertyField(_randomOrder_3);
            EditorGUILayout.PropertyField(_openPosOffset_3);
            if (_ctrl.openPosOffset_3)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_posOffset_3);
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
        }
    }
}
