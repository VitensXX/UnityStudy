using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//双人控制
public class DoublePlayerCtrl : MonoBehaviour
{
    public GameObject armyBlue;
    public GameObject armyRed;

    playerControl _armyBlueCtrl;
    playerControl _armyRedCtrl;
    Material _redMat;

    // Start is called before the first frame update
    void Start()
    {
        _armyBlueCtrl = armyBlue.GetComponentInChildren<playerControl>();
        _armyRedCtrl = armyRed.GetComponentInChildren<playerControl>();
        _redMat = armyRed.GetComponentInChildren<SkinnedMeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickAttack()
    {
        _armyBlueCtrl.Attack01();
        StartCoroutine(Hit());
    }

    public void OnClickKill()
    {
        _armyBlueCtrl.Attack03();
        StartCoroutine(Kill());
    }
    
    public void OnClickKill_2D()
    {
        _armyBlueCtrl.Attack03();
        StartCoroutine(Kill_2D());
    }

    IEnumerator Hit()
    {
        yield return new WaitForSeconds(0.462f);
        _armyRedCtrl.GetHit();
        _redMat.SetColor("_ColorGlitter", new Color(0.3f, 0.3f, 0.3f));
        yield return new WaitForSeconds(0.1f);
        _redMat.SetColor("_ColorGlitter", Color.black);
    }

    IEnumerator Kill()
    {
        yield return new WaitForSeconds(0.462f);
        _armyRedCtrl.Die();

        Shader killEffect = Shader.Find("Hidden/KillEffect");
        Time.timeScale = 0.15f;
        Camera.main.SetReplacementShader(killEffect, "BattleType");
        yield return new WaitForSecondsRealtime(3);
        //_redMat.SetColor("_ColorGlitter", Color.black);
        Time.timeScale = 1;
        Camera.main.ResetReplacementShader();
    }

    IEnumerator Kill_2D()
    {
        yield return new WaitForSeconds(0.462f);
        _armyRedCtrl.Die();

        Shader killEffect = Shader.Find("Hidden/KillEffect_2D");
        Time.timeScale = 0.15f;
        Camera.main.SetReplacementShader(killEffect, "BattleType");
        yield return new WaitForSecondsRealtime(3);
        //_redMat.SetColor("_ColorGlitter", Color.black);
        Time.timeScale = 1;
        Camera.main.ResetReplacementShader();
    }
}
