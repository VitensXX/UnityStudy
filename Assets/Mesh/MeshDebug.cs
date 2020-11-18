using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Created by Vitens on 2020/10/19 22:05:58
/// 
/// Description : 
///     网格调试
/// </summary>
[ExecuteInEditMode]
public class MeshDebug : MonoBehaviour
{
    public Transform test;
    Mesh _mesh;

    void Start()
    {
        _mesh = GetComponent<MeshFilter>().mesh;

       
    }

    void Update()
    {
        Vector3[] normals = _mesh.normals;
        Vector3[] vertices = _mesh.vertices;
        
        for (int i = 0; i < normals.Length; i++)
        {
            //平移
            Vector3 worldPos = vertices[i] + transform.position;
            //旋转
            worldPos = Rotate(worldPos, transform.eulerAngles);
            //if(i == 0)
            //{
            //    Debug.LogError(vertices[i] +"  "+ worldPos+" roate:"+ transform.eulerAngles);
            //}

            Vector3 normal = normals[i] + transform.position;
            normal = Rotate(normal, transform.eulerAngles);

            Debug.DrawRay(worldPos, normal, Color.green);


            //if(i == 0)
            //{
            //    test.position = worldPos;
            //}
        }

    }

    Vector3 Rotate(Vector3 pos, Vector3 rotation)
    {
        Vector3 rotatedPos = pos;
        float rx = -Mathf.PI / 180 * rotation.x;
        float ry = -Mathf.PI / 180 * rotation.y;
        float rz = -Mathf.PI / 180 * rotation.z;
        //x
        rotatedPos.y = pos.y * Mathf.Cos(rx) + pos.z * Mathf.Sin(rx);
        rotatedPos.z = -pos.y * Mathf.Sin(rx) + pos.z * Mathf.Cos(rx);
        pos = rotatedPos;
        //y
        rotatedPos.x = pos.x * Mathf.Cos(ry) - pos.z * Mathf.Sin(ry);
        rotatedPos.z = pos.x * Mathf.Sin(ry) + pos.z * Mathf.Cos(ry);
        pos = rotatedPos;
        //z
        rotatedPos.x = pos.x * Mathf.Cos(rz) + pos.y * Mathf.Sin(rz);
        rotatedPos.y = -pos.x * Mathf.Sin(rz) + pos.y * Mathf.Cos(rz);
        return rotatedPos;
    }
}
