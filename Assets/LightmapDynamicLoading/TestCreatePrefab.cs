using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCreatePrefab : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       GameObject prefab = GameObject.Instantiate<GameObject>(Resources.Load("TestPrefab") as GameObject);
       prefab.name = prefab.name.Replace("(Clone)","");

       prefab.AddComponent<LightmapDynamicLoading>();
    }

}
