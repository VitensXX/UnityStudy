using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookRotation : MonoBehaviour
{
   public GameObject Boss;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //保持和Boss同一个朝向
        transform.rotation = Quaternion.LookRotation(Boss.transform.forward);
        Debug.DrawRay(transform.position, Boss.transform.forward, Color.red);
    }
}
