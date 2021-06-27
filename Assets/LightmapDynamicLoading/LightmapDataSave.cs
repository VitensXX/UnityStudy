#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
public class LightmapDataSave : MonoBehaviour
{
    public Texture2D[] texLightmapLight;
    public Texture2D[] texLightmapDir;
    public Texture2D[] texShadowMask;

    public void Save(){
        string path = LightmapDataAsset.ASSET_PATH.Replace("prefabName", gameObject.name);
        if(File.Exists(path)){
            File.Delete(path);
            Debug.Log("delect old");
        }

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        LightmapDataAsset asset = ScriptableObject.CreateInstance<LightmapDataAsset>();

        asset.RecordTextures(texLightmapLight, texLightmapDir, texShadowMask);
        asset.RecordRendererInfo(renderers);
        //记录数据
        AssetDatabase.CreateAsset(asset, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}


[CustomEditor(typeof(LightmapDataSave))]
public class LightmapDataSaveInspector : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        
        if(GUILayout.Button("Save")) {
            ((LightmapDataSave)target).Save();
        }
    }
}
#endif
