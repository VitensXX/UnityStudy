using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Vitens.Editor;

/// <summary>
/// Created by Vitens on 2021/7/31 15:37:19
/// 
/// Description : 
///     自定义窗口测试脚本
/// </summary>
public class EditorWindowTest : VitensEditorWindow
{
    [MenuItem("Tools/EditorWindowTest")]
    static void Create()
    {
        EditorWindowTest window = (EditorWindowTest)GetWindow(typeof(EditorWindowTest), false, "EditorWindowTest");
        window.Show(true);
        window.Init();
    }

    List<int> _contents = new List<int>();
    protected override void Init(){
        base.Init(); 
        _contents.Clear();
        for (int i = 0; i < 10; i++)
        {
            _contents.Add(i);
        }

        RequestVector2s(1);
        RequestStrings(1);
        RequestBools(1);
        RequestColors(2);
    }

string textPast;

    private void OnGUI() {
        EditorGUILayout.BeginHorizontal("Toolbar");
        // EditorGUILayout.LabelField("Refresh", GUILayout.Width(60));
        if(GUILayout.Button("Refresh",new GUIStyle("toolbarbutton"), GUILayout.Width(60))){
            Init();
        }
        GUI.backgroundColor = Color.white;
        if(GUILayout.Button("Aa",new GUIStyle("toolbarbutton"), GUILayout.Width(25))){
            Debug.LogError("AAAAAAAA");
        }
        GUI.backgroundColor = Color.white;
        if(EditorGUILayout.DropdownButton(new GUIContent("test"), FocusType.Keyboard)){
            Debug.LogError("test");
        }
       
        EditorFragment.Extend();
        EditorGUILayout.EndHorizontal();
        


        // if(GUILayout.Button("ReInit", GUILayout.Height(40))){
        //     Init();
        // }
        EditorFragment.AssetSaveAndOpen("Assets/Editor/VitensEditor");
        strs[0] = EditorFragment.Search(strs[0], ReFocus);
        EditorGUILayout.LabelField(strs[0]);

        vector2s[0] = EditorGUILayout.BeginScrollView(vector2s[0], EditorGUIStyle.Box, GUILayout.MaxHeight(200));
        for (int i = 0; i < _contents.Count; i++)
        {
            if(_contents[i].ToString().Contains(strs[0])){
                EditorGUILayout.BeginHorizontal(EditorGUIStyle.Box);
                EditorFragment.BtnAdd(()=>{_contents.Add(i*i);});
                EditorGUILayout.LabelField(_contents[i].ToString(), GUILayout.Width(100));
                EditorFragment.BtnDelete(()=>{_contents.RemoveAt(i);});
                EditorFragment.Extend();
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndScrollView();

        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button("copy")){
            EditorUtils.CopyStr(strs[0]);
        }
    
        if(GUILayout.Button("paste")){
            textPast = EditorUtils.PastStr();
        }
        EditorGUILayout.LabelField("paste:"+textPast);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        // bools[0] = EditorGUILayout.Toggle(bools[0], new GUIStyle("BypassToggle"));
        bools[0] = EditorGUILayout.Toggle("Aa",bools[0], new GUIStyle("MuteToggle"));
        EditorGUILayout.LabelField(bools[0] ? "Yes" : "No");
        EditorGUILayout.LabelField(bools[0] ? "Yes" : "No", new GUIStyle("AM HeaderStyle"));
        EditorGUILayout.LabelField(bools[0] ? "Yes" : "No", new GUIStyle("AM MixerHeader"));
        EditorGUILayout.LabelField(bools[0] ? "Yes" : "No", new GUIStyle("AM MixerHeader2"));
        
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(position.width - 100);
        if(GUILayout.Button("控件靠右", GUILayout.Width(100))){

        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginHorizontal(EditorGUIStyle.Box);
        colors[0] = EditorGUILayout.ColorField(colors[0], GUILayout.Width(100));
        if(GUILayout.Button("copy", GUILayout.Width(50))){
            EditorUtils.CopyColor(colors[0]);
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal(EditorGUIStyle.Box);
        colors[1] = EditorGUILayout.ColorField(colors[1], GUILayout.Width(100));
        if(GUILayout.Button("paste", GUILayout.Width(50))){
            colors[1] = EditorUtils.PasteColor();
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndHorizontal();
        // if(GUILayout.Button("Aa", new GUIStyle("ControlHighlight"), GUILayout.Width(20), GUILayout.Height(20))){

        // }

        using(new BoxScope(true)){
            // editorgu
        }
    }
}
