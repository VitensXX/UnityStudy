using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{

    //双人控制
    public class DoublePlayerCtrl : MonoBehaviour
    {
        public GameObject armyBlue;
        public GameObject armyRed;

        ArmyView _armyViewBlue;
        ArmyView _armyViewRed;
        Material _redMat;

        // Start is called before the first frame update
        void Start()
        {
            _armyViewBlue = armyBlue.GetComponent<ArmyView>();
            _armyViewRed = armyRed.GetComponent<ArmyView>();
            _redMat = armyRed.GetComponentInChildren<SkinnedMeshRenderer>().material;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnClickAttack()
        {
            _armyViewBlue.SetAction(ArmyView.AnimAction.Attack01);
            StartCoroutine(Hit());
        }

        public void OnClickKill()
        {
            //_armyBlueCtrl.Attack03();
            _armyViewBlue.SetAction(ArmyView.AnimAction.Attack03);
            StartCoroutine(Kill());
        }

        public void OnClickKill_2D()
        {
            //_armyBlueCtrl.Attack03();
            _armyViewBlue.SetAction(ArmyView.AnimAction.Attack03);
            StartCoroutine(Kill_2D());
        }

        public void OnClickDying()
        {
            _armyViewBlue.SetAction(ArmyView.AnimAction.Attack02);
            StartCoroutine(Dying());
        }

        public void OnClickStopDying()
        {
            _armyViewRed.StopDying();
        }

        IEnumerator Hit()
        {
            yield return new WaitForSeconds(0.462f);
            //_armyRedCtrl.GetHit();
            _armyViewRed.SetAction(ArmyView.AnimAction.Hit);
            _armyViewRed.HitFlash();
        }

        IEnumerator Dying()
        {
            yield return new WaitForSeconds(0.462f);
            _armyViewRed.SetAction(ArmyView.AnimAction.Hit);
            _armyViewRed.HitFlash();
            _armyViewRed.Dying();
        }

        IEnumerator Kill()
        {
            yield return new WaitForSeconds(0.462f);
            //_armyRedCtrl.Die();
            _armyViewRed.SetAction(ArmyView.AnimAction.Die);

            Shader killEffect = Shader.Find("Hidden/KillEffect");
            Time.timeScale = 0.15f;//慢动作
            Camera.main.SetReplacementShader(killEffect, "BattleType");
            yield return new WaitForSecondsRealtime(3);
            Time.timeScale = 1;
            Camera.main.ResetReplacementShader();
        }

        IEnumerator Kill_2D()
        {
            yield return new WaitForSeconds(0.462f);
            _armyViewRed.SetAction(ArmyView.AnimAction.Die);

            Shader killEffect = Shader.Find("Hidden/KillEffect_2D");
            Time.timeScale = 0.15f;
            Camera.main.SetReplacementShader(killEffect, "BattleType");
            yield return new WaitForSecondsRealtime(3);
            //_redMat.SetColor("_ColorGlitter", Color.black);
            Time.timeScale = 1;
            Camera.main.ResetReplacementShader();
        }
    }
}