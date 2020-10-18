using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 显示3D模型轮廓线
/// </summary>
public class OutlineFor3D : MonoBehaviour
{
    //GameObject outlineGO;
    //SkinnedMeshRenderer _originRenderer;
    //MeshRenderer _outlineRenderer;
    //MeshFilter _outlineMeshFilter; 
    static int count = 0;
    [ContextMenu("test")]
    void Start()
    {
        GameObject outlineGO = GameObject.Instantiate(gameObject);
        outlineGO.transform.parent = transform.parent;
        //outlineGO.DestroyComponent<OutlineFor3D>();
        Destroy(outlineGO.GetComponent<OutlineFor3D>());

        SkinnedMeshRenderer outlineRenderer = outlineGO.GetComponent<SkinnedMeshRenderer>();
        outlineRenderer.material = new Material(Shader.Find("P03/OutlineFor3D"));
        Mesh outlineMesh = (Mesh)GameObject.Instantiate(outlineRenderer.sharedMesh);
        ProcessAveNormal(outlineMesh);
        outlineRenderer.sharedMesh = outlineMesh;

        //_originRenderer = GetComponent<SkinnedMeshRenderer>();
        //_outlineRenderer = outlineGO.AddComponent<MeshRenderer>();
        //_outlineRenderer.material = new Material(Shader.Find("P03/OutlineFor3D"));
        //_outlineMeshFilter = outlineGO.AddComponent<MeshFilter>();

        //SkinnedMeshRenderer smr = GetComponent<SkinnedMeshRenderer>();
        //if (smr)
        //{
        //    //SkinnedMeshRenderer outlineSMR = outlineGO.AddComponent<SkinnedMeshRenderer>();
        //    //Mesh tempMesh = (Mesh)GameObject.Instantiate(smr.sharedMesh);
        //    //ProcessAveNormal(tempMesh);
        //    //outlineSMR.sharedMesh = tempMesh;
        //    //outlineSMR.material = new Material(Shader.Find("P03/OutlineFor3D"));

        //    //MeshFilter meshFilter = outlineGO.AddComponent<MeshFilter>();
        //    //MeshRenderer meshRenderer = outlineGO.AddComponent<MeshRenderer>();
        //    //Mesh tempMesh = (Mesh)GameObject.Instantiate(smr.sharedMesh);
        //    //ProcessAveNormal(tempMesh);
        //    //meshFilter.sharedMesh = tempMesh;
        //    //meshRenderer.material = new Material(Shader.Find("P03/OutlineFor3D"));
        //}
        //else
        //{
        //    MeshFilter meshFilter = outlineGO.AddComponent<MeshFilter>();
        //    MeshRenderer meshRenderer = outlineGO.AddComponent<MeshRenderer>();
        //    Mesh tempMesh = (Mesh)GameObject.Instantiate(GetComponent<MeshFilter>().sharedMesh);
        //    ProcessAveNormal(tempMesh);
        //    meshFilter.sharedMesh = tempMesh;
        //    meshRenderer.material = new Material(Shader.Find("P03/OutlineFor3D"));
        //}


        //outlineGO.transform.localPosition = Vector3.zero;
        //outlineGO.transform.localEulerAngles = Vector3.zero;
        //outlineGO.transform.localScale = Vector3.one;
    }

    //Mesh tempMesh;
    //void Update()
    //{

    //    outlineGO.transform.localPosition = transform.localPosition;
    //    outlineGO.transform.localRotation = transform.localRotation;

    //    _originRenderer.BakeMesh(tempMesh);
    //    ProcessAveNormal(tempMesh);
    //    _outlineMeshFilter.mesh = tempMesh;

    //}



    public void ProcessAveNormal(Mesh mesh)
    {
        Dictionary<Vector3, List<int>> vertex2TriangleDic = new Dictionary<Vector3, List<int>>();
        for (int i = 0; i < mesh.vertexCount; i++)
        {
            Vector3 vertex = mesh.vertices[i];
            if (!vertex2TriangleDic.ContainsKey(vertex))
            {
                vertex2TriangleDic[vertex] = new List<int>();
            }

            vertex2TriangleDic[vertex].Add(i);
        }

        Vector3[] normals = mesh.normals;
        Vector3 tempNormal = Vector3.zero;
        foreach (var item in vertex2TriangleDic)
        {
            tempNormal = Vector3.zero;
            for (int i = 0; i < item.Value.Count; i++)
            {
                tempNormal += mesh.normals[item.Value[i]];
            }

            tempNormal /= item.Value.Count;
            for (int i = 0; i < item.Value.Count; i++)
            {
                normals[item.Value[i]] = tempNormal;
            }
        }

        //重新赋值平滑后的法线
        mesh.normals = normals;
    }
}
