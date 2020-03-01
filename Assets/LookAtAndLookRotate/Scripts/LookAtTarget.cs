using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    public GameObject Boss;//目标敌人

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Boss.transform);
        Debug.DrawLine(transform.position, Boss.transform.position, Color.blue);
    }
}
