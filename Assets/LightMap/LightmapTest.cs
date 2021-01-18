using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LightmapTest : MonoBehaviour
{
    public void OnClickLoadScene()
    {
        SceneManager.LoadScene("LightMapTest", LoadSceneMode.Additive);

        //Renderer renderer = GetComponent<Renderer>();
        //renderer.lightmapIndex = index;
        //renderer.lightmapScaleOffset = new Vector4(TilingX, TilingY, OffsetX, OffsetY);
        //        //int lightmapIndex = renderer.lightmapIndex;
        //        //Vector4 scaleOffset = r.lightmapScaleOffset.ToString("0.00000000");
        //        //Texture2D prefab = Resources.LoadAsync("Assets/Prefabs/Cube.prefab", typeof(Texture2D));
        //        //GameObject cube = (GameObject)Instantiate(prefab);
        //        //LightmapSettings.lightmaps = lightmapDatas;
        //LightmapData[] lightmapDatas = new LightmapData[count];
        //for (int i = 0; i < count; i++)
        //{
        //    LightmapData lightmapData = new LightmapData();
        //    lightmapData.lightmapColor = lightTxt;//加载"/Lightmap-" + i + "_comp_light.exr"
        //    lightmapData.lightmapDir = lightDir;//加载"/Lightmap-" + i + "_comp_dir.png"
        //    lightmapData.shadowMask = lightShadowMask;//加载 "/Lightmap-" + i + "_comp_shadowmask.png"
        //    lightmapDatas[i] = lightmapData;
        //}
        //        LightmapSettings.lightmaps = lightmapDatas;
    }
}
