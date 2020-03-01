using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookRotationTarget : MonoBehaviour
{
    public GameObject Boss;//目标敌人

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = Boss.transform.position - transform.position;
        dir = new Vector3(dir.x, 0, dir.z);
        transform.rotation = Quaternion.LookRotation(dir);
        Debug.DrawRay(transform.position, dir, Color.green);
    }
}
