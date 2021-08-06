using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Vitens.Editor;

public class LayoutTestWindow : EditorWindow
{
    [MenuItem("Tools/LayoutTestWindow")]
    static void Create()
    {
        LayoutTestWindow window = (LayoutTestWindow)GetWindow(typeof(LayoutTestWindow), false, "LayoutTestWindow");
        window.Show(true);
    }

    bool _fold;
    private void OnGUI() {
        //折叠缩进
        // _fold = EditorGUILayout.Foldout(_fold, "折叠");
        // if(_fold){
        //     EditorGUI.indentLevel += 1;
        //     EditorGUILayout.LabelField("test001");
        //     EditorGUILayout.LabelField("test002");
        //     // EditorGUILayout.BeginHorizontal();
        //     // GUILayout.Space(20);
        //     if(GUILayout.Button("按钮缩进", GUILayout.Width(100))){
        //         //do something
        //     }
        //     // EditorGUILayout.EndHorizontal();
        //     EditorGUILayout.LabelField("test004");
        //     EditorGUI.indentLevel -= 1;
        // }

        //靠右与居中
        // EditorGUILayout.LabelField("test001");
        // EditorGUILayout.BeginHorizontal();
        // GUILayout.Space(position.width - 100);
        // if(GUILayout.Button("按钮靠右", GUILayout.Width(100))){
        //     //do something
        // }
        // EditorGUILayout.EndHorizontal();
        // EditorGUILayout.BeginHorizontal();
        // GUILayout.Space((position.width - 100)/2);
        // if(GUILayout.Button("按钮居中",new GUIStyle("ButtonRight"), GUILayout.Width(100))){
        //     //do something
        //     Debug.LogError("1");
        // }
        // EditorGUILayout.EndHorizontal();

        //文本的靠右与居中
        // EditorGUILayout.LabelField("文本靠右", new GUIStyle("RightLabel"));
        // EditorGUILayout.LabelField("文本居中", new GUIStyle("CenteredLabel"));

        //交替显示
        // for (int i = 0; i < 10; i++)
        // {
        //     if(i % 2 == 0){
        //         EditorGUILayout.BeginHorizontal();
        //         GUILayout.Space(8);
        //     }
        //     else{
        //         EditorGUILayout.BeginHorizontal("box");
        //     }
        //     GUI.skin.label.fontSize = i * 2;
        //     EditorGUILayout.LabelField("index:"+i, GUILayout.Width(100));
        //     EditorGUILayout.LabelField("aaaaaaaaaaaaaa", GUILayout.Width(150));
        //     EditorGUILayout.LabelField("bbbbbbbbbbbbbb", GUILayout.Width(150));
        //     EditorGUILayout.LabelField("");
        //     EditorGUILayout.EndHorizontal();
        // }
        string[] selects ={"1","2"};
        selectIndex = GUILayout.SelectionGrid(selectIndex,selects, 3);

        EditorGUILayout.SelectableLabel("aaaa");
        EditorGUILayout.MinMaxSlider(ref minVal,ref  maxVal, 10, 100);
        EditorGUILayout.LabelField(minVal+"  "+ maxVal);

        m_SelectedPage=GUILayout.Toolbar(m_SelectedPage,m_ButtonStr,GUILayout.Height(25));


        GUILayout.Label ("Save Path", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.TextField(path,GUILayout.ExpandWidth(false));
        if(GUILayout.Button("Browse",GUILayout.ExpandWidth(false))){
            path = EditorUtility.SaveFolderPanel("Path to Save Images",path,Application.dataPath);
        }
        EditorGUILayout.EndHorizontal();
        
    }

string path = "";
    int selectedLayer=0;
    int selectIndex = 0;
    float minVal, maxVal; 
    int m_SelectedPage=0;
string[] m_ButtonStr=new string[4]{"Combine Animation","Check Part","Create RootMotion","CheckunUsedPrefab"};
}
