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
    public List<AnimationState> animClips;
    public string name;

    private Animation animation;
    private SkinnedMeshRenderer skin;

    #endregion

    public AnimData(Animation anim, SkinnedMeshRenderer smr, string goName)
    {
        vertexCount = smr.sharedMesh.vertexCount;
        animation = anim;
        mapWidth = vertexCount;//Mathf.NextPowerOfTwo(vertexCount);
        animClips = new List<AnimationState>();
        animClips.Add(animation["atk_qiang"]);
        // animClips = new List<AnimationState>(anim.Cast<AnimationState>());
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

        this.animation.Sample();
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

public class VertexAnimBakerV2 : EditorWindow
{
    public static GameObject targetGo;
    private Mesh _bakedMesh;

    [MenuItem("Window/AnimMapBakerV2")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(VertexAnimBakerV2));
        //baker = new AnimMapBaker();
        //animMapShader = Shader.Find("chenjd/AnimMapShader");
    }

    void OnGUI()
    {
        targetGo = (GameObject)EditorGUILayout.ObjectField(targetGo, typeof(GameObject), true);

        if (GUILayout.Button("Bake"))
        {
            Debug.LogError("bake");
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

        //Animation anim = go.GetComponentInChildren<Animation>();
        Animation anim = go.GetComponent<Animation>();
        SkinnedMeshRenderer smr = go.GetComponentInChildren<SkinnedMeshRenderer>();

        if (anim == null || smr == null)
        {
            Debug.LogError("anim or smr is null!!");
            return;
        }
        _bakedMesh = new Mesh();
        AnimData animData = new AnimData(anim, smr, go.name);

        BakeAnimClip(animData.animClips[0], animData);
    }

    private void BakeAnimClip(AnimationState animState, AnimData animData)
    {
        Debug.LogError("bakeAnimClip");
        int curClipFrame = (int)(animState.clip.frameRate * animState.length);
        float perFrameTime = animState.length / curClipFrame; ;
        float sampleTime = 0;

        Texture2D animMapTex = new Texture2D(animData.mapWidth, curClipFrame, TextureFormat.RGBAHalf, false);
        // Texture2D normalMapTex = new Texture2D(animData.mapWidth, curClipFrame, TextureFormat.RGBAHalf, false);
        animMapTex.name = animState.name + ".animMap";
        // normalMapTex.name = animState.name + ".normalMap";
        animData.AnimationPlay(animState.name);


        Vector3[] vertices = _bakedMesh.vertices;
        List<int> indexByUV = new List<int>();
        Vector2[] uv = _bakedMesh.uv;
        for (int i = 0; i < uv.Length; i++)
        {
            int index = Mathf.RoundToInt(uv[i].x / 0.166f) + Mathf.RoundToInt(uv[i].y / 0.125f) * 7;
            indexByUV.Add(index);
        }


        for (int i = 0; i < curClipFrame; i++)
        {
            animState.time = sampleTime;
            animData.SampleAnimAndBakeMesh(ref _bakedMesh);

            for (int j = 0; j < _bakedMesh.vertexCount; j++)
            {
                int index = indexByUV[j];
                // Vector3 vertex = _bakedMesh.vertices[j];
                Vector3 vertex = vertices[index];
                // Vector3 normal = _bakedMesh.normals[j];
                animMapTex.SetPixel(index, i, new Color(vertex.x, vertex.y, vertex.z));
                // normalMapTex.SetPixel(j, i, new Color(normal.x, normal.y, normal.z));
            }

            sampleTime += perFrameTime;
        }
        animMapTex.Apply();
        // normalMapTex.Apply();

        animState.time = 0;
        animData.SampleAnimAndBakeMesh(ref _bakedMesh);

Debug.LogError("保存成功");
        //保存顶点位子映射图
        AssetDatabase.CreateAsset(animMapTex, "Assets/"+animData.name+animData.animClips[0].name +"_vertexMapV2.asset");
        // AssetDatabase.CreateAsset(normalMapTex, "Assets/GPU instancing/VertexAnim/"+animData.name+animData.animClips[0].name +"_normalMapV2.asset");
    }
}
