using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LightmapDynamicLoading : MonoBehaviour
{
    public LightmapDataAsset lightmapDataAsset;

    void Start()
    {
        //LightmapSettings的参数设置
        int count = lightmapDataAsset.texLightmapLight.Length;
        LightmapData[] lightmapDatas = new LightmapData[lightmapDataAsset.texLightmapLight.Length];
        for (int i = 0; i < count; i++)
        {
            LightmapData lightmapData = new LightmapData();
            lightmapData.lightmapColor = lightmapDataAsset.texLightmapLight[i];
            // lightmapData.lightmapDir = asset.texLightmapDir[i];
            // lightmapData.shadowMask = asset.texShadowMask[i];
            lightmapDatas[i] = lightmapData;
        }
        LightmapSettings.lightmaps = lightmapDatas;

        //每个Renderer的参数设置
        Renderer[] renderer = GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renderer.Length; i++)
        {
            if(lightmapDataAsset.index.Length > i){
                renderer[i].lightmapIndex = lightmapDataAsset.index[i];
                renderer[i].lightmapScaleOffset = lightmapDataAsset.scaleAndOffset[i];
            }
        }
    }
}
