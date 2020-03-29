using System.Collections;
using System.Collections.Generic;
using UnityEditor;
//using System.Linq;
using UnityEngine;


public class AnimData
{
    #region 字段

    public int vertexCount;
    public int mapWidth;
    //public List<AnimationState> animClips;
    public string name;

    private Animator animation;
    private SkinnedMeshRenderer skin;

    #endregion

    public AnimData(Animator anim, SkinnedMeshRenderer smr, string goName)
    {
        vertexCount = smr.sharedMesh.vertexCount;
        mapWidth = Mathf.NextPowerOfTwo(vertexCount);
        //animClips = new List<AnimationState>(anim.Cast<AnimationState>());
        //Debug.LogError("animclips count " + animClips.Count);
        animation = anim;
        skin = smr;
        name = goName;
    }


    public void AnimationPlay(string animName)
    {
        this.animation.Play(animName);
    }

    public void SampleAnimAndBakeMesh(ref Mesh m)
    {
        this.SampleAnim();
        this.BakeMesh(ref m);
    }

    private void SampleAnim()
    {
        if (this.animation == null)
        {
            Debug.LogError("animation is null!!");
            return;
        }

        //this.animation.Sample();
    }

    private void BakeMesh(ref Mesh m)
    {
        if (this.skin == null)
        {
            Debug.LogError("skin is null!!");
            return;
        }

        this.skin.BakeMesh(m);
    }
}

/// <summary>
/// 烘焙后的数据
/// </summary>
public struct BakedData
{
    public string name;
    public float animLen;
    public byte[] rawAnimMap;
    public int animMapWidth;
    public int animMapHeight;

    public BakedData(string name, float animLen, Texture2D animMap)
    {
        this.name = name;
        this.animLen = animLen;
        this.animMapHeight = animMap.height;
        this.animMapWidth = animMap.width;
        this.rawAnimMap = animMap.GetRawTextureData();
    }
}

public class VertexAnimBaker : EditorWindow
{
    public static GameObject targetGo;
    private Mesh _bakedMesh;

    [MenuItem("Window/AnimMapBaker")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(VertexAnimBaker));
        //baker = new AnimMapBaker();
        //animMapShader = Shader.Find("chenjd/AnimMapShader");
    }

    void OnGUI()
    {
        targetGo = (GameObject)EditorGUILayout.ObjectField(targetGo, typeof(GameObject), true);

        if (GUILayout.Button("Bake"))
        {
            InitAnimData(targetGo);
        }
    }


    private void InitAnimData(GameObject go)
    {
        if (go == null)
        {
            Debug.LogError("go is null!!");
            return;
        }

        Animator anim = go.GetComponentInChildren<Animator>();
        SkinnedMeshRenderer smr = go.GetComponentInChildren<SkinnedMeshRenderer>();

        if (anim == null || smr == null)
        {
            Debug.LogError("anim or smr is null!!");
            return;
        }
        _bakedMesh = new Mesh();
        AnimData animData = new AnimData(anim, smr, go.name);

        BakeAnimClip(anim.GetCurrentAnimatorClipInfo(0)[0].clip, animData);
    }

    private void BakeAnimClip(AnimationClip animState, AnimData animData)
    {
        int curClipFrame = (int)( animState.frameRate * animState.length);
        float perFrameTime = animState.length / curClipFrame; ;
        float sampleTime = 0;

        Texture2D animMapTex = new Texture2D(animData.mapWidth, curClipFrame, TextureFormat.RGBAHalf, false);
        animMapTex.name = animState.name + ".animMap";
        animData.AnimationPlay(animState.name);

        for (int i = 0; i < curClipFrame; i++)
        {
            //animState.time = sampleTime;
            animState.SampleAnimation(targetGo, sampleTime);

            animData.SampleAnimAndBakeMesh(ref _bakedMesh);

            for (int j = 0; j < _bakedMesh.vertexCount; j++)
            {
                Vector3 vertex = _bakedMesh.vertices[j];
                animMapTex.SetPixel(j, i, new Color(vertex.x, vertex.y, vertex.z));
            }

            sampleTime += perFrameTime;
        }
        animMapTex.Apply();
        //保存顶点位子映射图
        AssetDatabase.CreateAsset(animMapTex, "Assets/GPU instancing/VertexAnim/"+animData.name +".asset");
    }
}
