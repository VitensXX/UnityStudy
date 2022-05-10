using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TestSDFMaker : MonoBehaviour
{
    public Texture2D sorce;
    public Texture2D dest;
    public Texture2D sdfPreviewTexture;
    public int distance;
    [Range(0,1)]
    public float AlphaThreshold;

    float[,] originAlpha;
    int width;
    int height;

    // Start is called before the first frame update
    void Start()
    {
        // sorce.GetPixel()
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Test(){
       

        width = sorce.width;
        height = sorce.height;
        int distanceSq = distance * distance;

        originAlpha = new float[width, height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                originAlpha[x,y] = sorce.GetPixel(x,y).a;
            }
        }

        sdfPreviewTexture = new Texture2D(width, height);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float minDisSq = Distance(x, y);
                sdfPreviewTexture.SetPixel(x,y, Color.white * (1 - minDisSq / distanceSq));
            }
        }
        sdfPreviewTexture.Apply();


        Debug.LogError("Apply");
        // SDFImageMaker.GenerateSDF(sorce,dest,distance);
        SDFImageMaker.GenerateBinaryImage(sdfPreviewTexture);
        AssetDatabase.Refresh();
    }

    bool IsPixelSafe(int x, int y){
        return x < width && x >= 0 && y < height && y >= 0;  
    }

    int DistanceSq(int x, int y){
        return Mathf.Abs(x) * Mathf.Abs(x) + Mathf.Abs(y) * Mathf.Abs(y);
    }

    int Distance(int x, int y){
        int halfDis = distance;
        int minDis = distance * distance;
        for (int offsetX = -halfDis; offsetX < halfDis; offsetX++)
        {
            for (int offsetY = -halfDis; offsetY < halfDis; offsetY++)
            {
                int tempX = x + offsetX;
                int tempY = y + offsetY;

                if(!IsPixelSafe(tempX,tempY)){
                    continue;
                }
                
                if(originAlpha[tempX,tempY] >= AlphaThreshold){
                    int curDis = DistanceSq(offsetX, offsetY);
                    if(minDis > curDis){
                        minDis = curDis;
                    }
                }
            }
        }

        return minDis;
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(TestSDFMaker))]
public class TestSDFMakerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        TestSDFMaker sdfMaker = (TestSDFMaker)target;
        if(GUILayout.Button("test")){
            sdfMaker.Test();
        }
        GUILayout.Label(sdfMaker.sdfPreviewTexture);
        // GUILayout.BeginHorizontal(sdfMaker.sorce);
        // GUILayout.EndArea();
        // GUI.DrawTexture(new Rect(100,100, 100, 100), sdfMaker.sorce);
        // GUILayout.
    }
}


#endif
