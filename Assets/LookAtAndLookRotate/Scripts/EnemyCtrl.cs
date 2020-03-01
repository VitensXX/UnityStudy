using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
    enum Dir { forward, back, left, right, up, down}

    public float moveSpeed = 0.1f;
    public float roateSpeed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Move(Dir.forward);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Move(Dir.back);
        }
        if (Input.GetKey(KeyCode.A))
        {
            Move(Dir.left);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Move(Dir.right);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Move(Dir.up);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Move(Dir.down);
        }


        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Rotate(Dir.left);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            Rotate(Dir.right);
        }
        
    }

    private void Move(Dir dir)
    {
        switch (dir)
        {
            case Dir.forward:
                transform.position += transform.forward * moveSpeed;
                break;
            case Dir.back:
                transform.position -= transform.forward * moveSpeed;
                break;
            case Dir.left:
                transform.position -= transform.right * moveSpeed;
                break;
            case Dir.right:
                transform.position += transform.right * moveSpeed;
                break;
            case Dir.up:
                transform.position += transform.up * moveSpeed;
                break;
            case Dir.down:
                transform.position -= transform.up * moveSpeed;
                break;
            default:
                break;
        }
    }

    private void Rotate(Dir dir)
    {
        if(dir == Dir.left)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(-transform.right), roateSpeed);
        }
        else if(dir == Dir.right)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.right), roateSpeed);
        }
    }
}
