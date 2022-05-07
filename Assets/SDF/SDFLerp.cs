using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SDFLerp : MonoBehaviour
{
    public Texture2D SDF_A;
    public Texture2D SDF_B;
    public Texture2D sdfPreviewTexture;
    public Texture2D finalTexture;

    [Range(0,1)]
    public float lerp = 0;
    [Range(0,1)]
    public float alphaThreshold = 0.8f;
    public float alphaThreshold2 = 0.1f;
    public int power = 4;
    int width;
    int height;

    // Start is called before the first frame update
    void Start()
    {
        width = SDF_A.width;
        height = SDF_A.height;
        sdfPreviewTexture = new Texture2D(width, height);
        finalTexture = new Texture2D(width, height);
    }

    // Update is called once per frame
    void Update()
    {
        Lerp();
    }

    void Lerp(){
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Color a = SDF_A.GetPixel(x,y);
                Color b = SDF_B.GetPixel(x,y);
                float alpha = Mathf.Lerp(a.a,b.a, lerp);
                sdfPreviewTexture.SetPixel(x,y, Color.white * (alpha));

               if(alpha < alphaThreshold){
                    finalTexture.SetPixel(x, y, Color.white * 0);
                }
                else{
                    finalTexture.SetPixel(x, y, Color.white);
                }
            }
        }
        sdfPreviewTexture.Apply();
        finalTexture.Apply();
    }
}


#if UNITY_EDITOR

[CustomEditor(typeof(SDFLerp))]
public class SDF_LerpEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SDFLerp sdfMaker = (SDFLerp)target;
        // if(GUILayout.Button("test")){
        //     sdfMaker.Test();
        // }
        GUILayout.Label(sdfMaker.sdfPreviewTexture);
        GUILayout.Label(sdfMaker.finalTexture);
        // GUILayout.BeginHorizontal(sdfMaker.sorce);
        // GUILayout.EndArea();
        // GUI.DrawTexture(new Rect(100,100, 100, 100), sdfMaker.sorce);
        // GUILayout.
    }
}


#endif