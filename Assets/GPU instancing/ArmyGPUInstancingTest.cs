using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyGPUInstancingTest : MonoBehaviour
{
    public GameObject Army;
    
    Transform _tran;

    Dictionary<int, MeshRenderer> _armyId2Renderer;

    // Start is called before the first frame update
    void Start()
    {
        _tran = transform;
        _armyId2Renderer = new Dictionary<int, MeshRenderer>();
        CreateArmy();
    }

    void CreateArmy()
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject armyInst = Instantiate<GameObject>(Army);
            armyInst.transform.localPosition = new Vector3(i * 3, 0, 0);
            MeshRenderer renderer = armyInst.GetComponentInChildren<MeshRenderer>();
            _armyId2Renderer.Add(i, renderer);
        }
        for (int i = 0; i < 2; i++)
        {
            MaterialPropertyBlock props = new MaterialPropertyBlock();
            //props.SetFloat("_AnimState", i);
            props.SetFloat("_AnimLen", i+1);

            float r = Random.Range(0.0f, 1.0f);
            float g = Random.Range(0.0f, 1.0f);
            float b = Random.Range(0.0f, 1.0f);
            props.SetColor("_Tint", new Color(r, g, b));

            _armyId2Renderer[i].SetPropertyBlock(props);
        }
    }
}
