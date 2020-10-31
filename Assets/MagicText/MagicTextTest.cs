using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicTextTest : MonoBehaviour
{

    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 50), "Replay"))
        {
            MagicText[] magics = GetComponentsInChildren<MagicText>();
            for (int i = 0; i < magics.Length; i++)
            {
                magics[i].Replay();
            }
        }

        if (GUI.Button(new Rect(120, 0, 100, 50), "Stop"))
        {
            MagicText[] magics = GetComponentsInChildren<MagicText>();
            for (int i = 0; i < magics.Length; i++)
            {
                magics[i].Stop();
            }
        }
    }

}
