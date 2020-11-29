using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Created by Vitens on 2020/11/29 14:54:24
/// 
/// Description : 
///     UGUI文本上标效果
/// </summary>
[ExecuteInEditMode]
public class Superscript : MonoBehaviour
{
    public string label = "";
    Text text;
    void Start()
    {
        text = GetComponent<Text>();
        //text.text = ProcessSuperscript(text.text);
    }

    void Update()
    {
        text.text = ProcessSuperscript(label);
    }

    //上标显示 支持 0-9  + - = ( )
    string ProcessSuperscript(string str)
    {
        MatchCollection matchedTags = Regex.Matches(str, "\\^.");
        foreach (Match matchedTag in matchedTags)
        {
            string tag = matchedTag.ToString();
            string v = tag[1].ToString();
            str = str.Replace(tag, GetSuperscriptUnicode(v));
        }
        return str;
    }

    string GetSuperscriptUnicode(string v)
    {
        switch (v)
        {
            case "0": return "\u2070";
            case "1": return "\u00B9";
            case "2": return "\u00B2";
            case "3": return "\u00B3";
            case "4": return "\u2074";
            case "5": return "\u2075";
            case "6": return "\u2076";
            case "7": return "\u2077";
            case "8": return "\u2078";
            case "9": return "\u2079";
            case "+": return "\u207A";
            case "-": return "\u207B";
            case "=": return "\u207C";
            case "(": return "\u207D";
            case ")": return "\u207E";
            default: return v;
        }
    }
}
