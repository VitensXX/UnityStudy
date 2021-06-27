using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 存储烘焙后的信息 存在Json中比较靠谱
// 1.烘焙后生成的光照贴图（存路径或索引就行）
// 2.每个Renderer上的参数，因为没有建立太好的对应关系，所以临时就通过GetComponentsInChildren方式获得的Renderer[]的默认索引顺序存储
[CreateAssetMenu(menuName = "ScriptableObjects/CreateCubeScriptableObject")]
public class LightmapDataAsset : ScriptableObject
{
    public const string ASSET_PATH = @"Assets/LightmapDynamicLoading/prefabName.asset";

    public Texture2D[] texLightmapLight;
    public Texture2D[] texLightmapDir;
    public Texture2D[] texShadowMask;

    #region 每个Renderer上的光照贴图数据
    public int[] index;
    public Vector4[] scaleAndOffset;
    #endregion

    //记录烘焙贴图（通过路径配合index的方式加载更合理，我本地测试工程就直接通过外面手动拖的方式建立关联）
    public void RecordTextures(Texture2D[] texLightmapLight, Texture2D[] texLightmapDir, Texture2D[] texShadowMask){
        this.texLightmapLight = texLightmapLight;
        this.texLightmapDir = texLightmapDir;
        this.texShadowMask = texShadowMask;
    }

    //记录Renderer上面的信息 index，scaleOffset
    public void RecordRendererInfo(Renderer[] renderers){
        index = new int[renderers.Length];
        scaleAndOffset = new Vector4[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            index[i] = renderers[i].lightmapIndex;
            scaleAndOffset[i] = renderers[i].lightmapScaleOffset;
        }
    }
}
