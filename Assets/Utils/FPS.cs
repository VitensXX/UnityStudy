using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPS : MonoBehaviour
{
    int frames;
    float fps;
    float _lastInterval;
    float _updateInterval = 0.2f;

    Text fpsText;
    private void Start()
    {
        fpsText = GameObject.Find("FPS").GetComponent<Text>();
    }
    // Update is called once per frame
    void Update()
    {
        frames++;
        float timeNow = Time.realtimeSinceStartup;
        if (timeNow > _lastInterval + _updateInterval)
        {
            fps = (float)(frames / (timeNow - _lastInterval));
            frames = 0;
            _lastInterval = timeNow;
        }

        fpsText.text = "FPS: "+ fps.ToString("0");
    }

    //private void OnGUI()
    //{
    //    GUI.Label(new Rect(0, 0, 100, 30), "FPS: " + fps.ToString("0"));
    //}
}
