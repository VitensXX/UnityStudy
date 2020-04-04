using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShaderFeatureAndMultiCompileTest : MonoBehaviour
{
    public Image shaderFeature;
    public Image multiCompile;

    bool _isRed = false;
    Material _matShaderFeature;
    Material _matMultiCompile;

    // Start is called before the first frame update
    void Start()
    {
        _matShaderFeature = shaderFeature.material;
        _matMultiCompile = multiCompile.material;
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(100, 100, 100, 80), "Red"))
        {
            _matShaderFeature.EnableKeyword("_SHOW_RED_ON");
            _matMultiCompile.EnableKeyword("_SHOW_RED_ON");
        }
        if (GUI.Button(new Rect(250, 100, 100, 80), "White"))
        {
            _matShaderFeature.DisableKeyword("_SHOW_RED_ON");
            _matMultiCompile.DisableKeyword("_SHOW_RED_ON");
        }
    }

}
