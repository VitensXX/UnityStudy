using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class Calc : MonoBehaviour
{
    public Text input;
    public bool calc;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(calc){
            calc = false;
            Do();
        }
    }

    void Do(){
        string s = input.text;
        s = s.Replace("+", " ");
        string[] arr = s.Split(' ');
        int sum = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            sum += IsWholeNumber(arr[i]);
        }
        Debug.LogError(sum);
    }

    /// <summary>
    /// 数字匹配
    /// </summary>
    /// <param name="strNumber"></param>
    /// <returns></returns>
    public static int IsWholeNumber(string strNumber)
    {
        System.Text.RegularExpressions.Regex g = new System.Text.RegularExpressions.Regex(@"[0-9]+");
        return int.Parse(g.Match(strNumber).Value);
        // if(g.IsMatch(strNumber)){
        //     return g.Match().Result();
        // }
    }
}
