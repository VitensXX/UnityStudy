using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPUInstancing
{
    public class CameraMove : MonoBehaviour
    {
        public bool Stop;
        public bool Pause;
        float _moveZ = -48;
        float _speed = 2;

        // Update is called once per frame
        void Update()
        {
            if (Stop)
            {
                _moveZ = -48;
                return;
            }
            if (Pause)
            {
                return;
            }
            _moveZ += Time.deltaTime * _speed;
            transform.position = new Vector3(0, 0, _moveZ);
        }
    }
}

