using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPUInstancingTest : MonoBehaviour
{
    Transform _tran;

    // Start is called before the first frame update
    void Start()
    {
        _tran = transform;
        CreateSpheres();
        SetColor();
    }

    //批量创建
    void CreateSpheres()
    {
        for (int i = 0; i < 10000; i++)
        {
            GameObject sphere = Instantiate(Resources.Load<GameObject>("Sphere"));
            float x = Random.Range(-1f, 1f);
            float y = Random.Range(-1f, 1f);
            float z = Random.Range(-1f, 1f);

            sphere.transform.parent = _tran;
            sphere.transform.position = new Vector3(x, y, z) * 30;
            sphere.transform.localScale = Vector3.one * Random.Range(0.5f, 1.5f);
        }
    }

    void SetColor()
    {
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        MaterialPropertyBlock props = new MaterialPropertyBlock();
        for (int i = 0; i < renderers.Length; i++)
        {
            float r = Random.Range(0.0f, 1.0f);
            float g = Random.Range(0.0f, 1.0f);
            float b = Random.Range(0.0f, 1.0f);
            props.SetColor("_Tint", new Color(r, g, b));
            renderers[i].SetPropertyBlock(props);
        }
    }
}
