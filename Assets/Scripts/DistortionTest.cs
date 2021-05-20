using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistortionTest : MonoBehaviour
{
    public Slider slider;
    public Text fmod;
    Material _mat;
    bool _timeFmod = false;
    // Start is called before the first frame update
    void Start()
    {
        _mat = GetComponent<Renderer>().material;
        ResetFmod();
    }

    private void OnEnable() {
        slider.onValueChanged.AddListener((strength)=>{
            _mat.SetFloat("_Strength", strength * 5); 
        });
    }
    
    private void OnDisable() {
        slider.onValueChanged.RemoveAllListeners();
    }

    public void OnClickChangeTimeFmod(){
        _timeFmod = !_timeFmod;
        ResetFmod();
    }

    void ResetFmod(){
        if(_timeFmod){
            _mat.EnableKeyword("TimeFomd");
            fmod.text = "开启Fmod";
        }
        else{
            _mat.DisableKeyword("TimeFomd");
            fmod.text = "关闭Fmod";
        }
    }

}
