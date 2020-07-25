using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System;
using System.Text.RegularExpressions;

/// <summary>
/// Created by Vitens on 2020/7/25 13:14:55
/// 
/// 创建脚本窗口
/// </summary>
public class CreateScriptWindow : EditorWindow
{
    const string TEMPLATE_PATH = "Assets/Platform/Editor/MyTemplateScript.txt";//模板路径

    [MenuItem("Tools/Create Script")]
    static void Create()
    {
        CreateScriptWindow window = (CreateScriptWindow)GetWindow(typeof(CreateScriptWindow), false, "Create Script");
        window.Show(true);
    }

    string _scriptName;
    string _description;
    void OnGUI()
    {
        //获取选中的文件夹路径
        string selectDirPath = EditorUtils.GetOneSelectFilePath();
        //如果选中的是文件夹,则正常拼接新建脚本路径
        if(Directory.Exists(selectDirPath))
        {
            _scriptName = EditorGUILayout.TextField("脚本名:", _scriptName);
            _description = EditorGUILayout.TextField("功能描述(选填):", _description);
            string path = selectDirPath + "/" + _scriptName + ".cs";
            EditorGUILayout.LabelField("路径:", path);
            EditorGUILayout.Space();
            if (GUILayout.Button("创建", GUILayout.Height(40)))
            {
                //命名规则校验
                if (!CheckScriptName(_scriptName))
                {
                    ShowNotification(new GUIContent("请输入正确的脚本名!"));   
                }
                //查重校验
                else if (CheckRepeat(selectDirPath))
                {
                    ShowNotification(new GUIContent("当前文件夹下已经有同名脚本!"));
                }
                //生成脚本
                else
                {
                    string content = File.ReadAllText(TEMPLATE_PATH);//从模板文件中读取内容
                    content = content.Replace("MyScript", _scriptName); //替换脚本名
                    content = content.Replace("time", DateTime.Now.ToString());//替换创建时间
                    content = content.Replace("ReplaceDescription", _description);//填入脚本功能描述
                    File.WriteAllText(path, content, Encoding.UTF8);//将修改后的内容写入新的脚本
                    AssetDatabase.Refresh();//马上刷新,方便在Project中直接看到新生成的脚本
                    ShowNotification(new GUIContent("Success!"));
                }
            }
        }
        else
        {
            EditorGUILayout.LabelField("请在Project面板中选择将要放置脚本的文件夹.");
        }
    }

    //校验脚本名(只能包含数字,字母,下划线且必须以字母或下划线开头)
    bool CheckScriptName(string scriptName)
    {
        Regex regex = new Regex("^[a-zA-Z_][a-zA-Z0-9_]*$");
        return regex.IsMatch(scriptName);
    }

    //脚本重复校验
    bool CheckRepeat(string selectDirPath)
    {
        DirectoryInfo dir = new DirectoryInfo(selectDirPath);
        FileInfo[] files = dir.GetFiles();
        string scriptName = _scriptName + ".cs";
        for (int i = 0; i < files.Length; i++)
        {
            if(files[i].Name == scriptName)
            {
                return true;
            }
        }

        return false;
    }

    //鼠标选中发生变化时调用
    private void OnSelectionChange()
    {
        //重绘窗口,刷新显示
        Repaint();
    }
}
