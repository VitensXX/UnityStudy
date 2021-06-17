using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CubeScriptableObjectTest : MonoBehaviour
{
    public CubeScriptableObject redCubeSO;
    public CubeScriptableObject greenCubeSO;
    public GameObject Cube;
    // Start is called before the first frame update
    void Start()
    {
        CreateCube(redCubeSO);
        CreateCube(greenCubeSO);
        LoadAsset();
    }

    //从ScriptableObject文件中获取
    void CreateCube(CubeScriptableObject cubeSO){
        GameObject cubeGO = GameObject.Instantiate<GameObject>(Cube);
        Transform ts = cubeGO.transform;
        //读取CubeScriptableObject中的数据并赋值到新建的Cube上
        ts.position = cubeSO.position;
        ts.localScale = cubeSO.scale;
        ts.localEulerAngles = cubeSO.rotation;
        cubeGO.GetComponent<Renderer>().material.color = cubeSO.color;
    }

    //编辑器下读取
    void LoadAsset(){
        CubeScriptableObject cubeAsset = AssetDatabase.LoadAssetAtPath<CubeScriptableObject>(@"Assets/ScriptableObject/blueCube.asset");
        //do something
        
    }

    //保存
    void SaveAsset(){
        CubeScriptableObject cubeAsset = ScriptableObject.CreateInstance<CubeScriptableObject>();
        cubeAsset.position = new Vector3(1,2,3);
        cubeAsset.rotation = new Vector3(4,5,6);
        cubeAsset.scale = new Vector3(1,1,1);
        cubeAsset.color = Color.blue;
#if UNITY_EDITOR
        AssetDatabase.CreateAsset(cubeAsset, @"Assets/ScriptableObject/blueCube.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
#endif
    }

}
