using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshVertexIndexTest : MonoBehaviour
{
    public Mesh mesh;
    
    [ContextMenu("DisplayIndexInfo")]
    void DisplayIndexInfo(){
        if(mesh == null){
            return;
        }

        Vector2[] uv = mesh.uv;
        string log = "";
        for (int i = 0; i < uv.Length; i++)
        {
            log += "["+i.ToString("00")+"]" + uv[i].ToString("0.00")+"  ";
            if(i % 11 == 10){
                log += "\n";
            }
        }
        Debug.LogError(log);
    }
}
