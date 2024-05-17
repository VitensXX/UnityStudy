using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Joker
{

    public class ShaderPropertyToID
    {
        public static int _Factor = Shader.PropertyToID("_Factor");
        public static int _Speed = Shader.PropertyToID("_Speed");
        public static int _Mask = Shader.PropertyToID("_Mask");
    }
}
