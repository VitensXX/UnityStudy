using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LightmapDynamicLoading : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 应该通过加载文件的方式读取数据（比如存在json中） 下面这个仅仅编辑器能用
        LightmapDataAsset asset = UnityEditor.AssetDatabase.LoadAssetAtPath<LightmapDataAsset>(LightmapDataAsset.ASSET_PATH.Replace("prefabName", gameObject.name.Replace("(Clone)","")));

        //LightmapSettings的参数设置
        int count = asset.texLightmapLight.Length;
        LightmapData[] lightmapDatas = new LightmapData[asset.texLightmapLight.Length];
        for (int i = 0; i < count; i++)
        {
            LightmapData lightmapData = new LightmapData();
            lightmapData.lightmapColor = asset.texLightmapLight[i];
            // lightmapData.lightmapDir = asset.texLightmapDir[i];
            // lightmapData.shadowMask = asset.texShadowMask[i];
            lightmapDatas[i] = lightmapData;
        }
        LightmapSettings.lightmaps = lightmapDatas;

        //每个Renderer的参数设置
        Renderer[] renderer = GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renderer.Length; i++)
        {
            if(asset.index.Length > i){
                renderer[i].lightmapIndex = asset.index[i];
                renderer[i].lightmapScaleOffset = asset.scaleAndOffset[i];
            }
        }
    }
}
