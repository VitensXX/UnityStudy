using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TestSDFMaker : MonoBehaviour
{
    public Texture2D sorce;
    public Texture2D dest;
    public int distance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Test(){
        // SDFImageMaker.GenerateSDF(sorce,dest,distance);
        SDFImageMaker.GenerateBinaryImage(sorce);
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(TestSDFMaker))]
public class TestSDFMakerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("test")){
            ((TestSDFMaker)target).Test();
        }
    }
}


#endif
