using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class ArmyView : MonoBehaviour
    {
        public enum AnimAction
        {
            Idle,
            Attack01,
            Attack02,
            Attack03,
            Hit,
            Die
        }

        playerControl _animCtrl;
        Material _mat;

        // Start is called before the first frame update
        void Start()
        {
            //GO_missionDetails = transform.Find("Content/GO_missionDetails").gameObject;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetAction(AnimAction action)
        {
            switch (action)
            {
                case AnimAction.Attack01:
                    _animCtrl.Attack01();
                    break;
                case AnimAction.Attack02:
                    _animCtrl.Attack02();
                    break;
                case AnimAction.Attack03:
                    _animCtrl.Attack03();
                    break;
                case AnimAction.Idle:
                    _animCtrl.Idle02();
                    break;
                case AnimAction.Die:
                    _animCtrl.Die();
                    break;
                case AnimAction.Hit:
                    _animCtrl.GetHit();
                    break;
                default:
                    break;
            }
        }

        public void HitFlash()
        {
            StartCoroutine(hitFlash());
        }

        IEnumerator hitFlash()
        {
            _mat.SetColor("_ColorGlitter", new Color(0.3f, 0.3f, 0.3f));
            yield return new WaitForSeconds(0.1f);
            _mat.SetColor("_ColorGlitter", Color.black);
        }

        Coroutine _dying;
        public void Dying()
        {
            _dying = StartCoroutine(dying());
        }

        public void StopDying()
        {
            StopCoroutine(_dying);
        }

        IEnumerator dying()
        {
            while (true)
            {
                _mat.SetColor("_ColorGlitter", new Color(0.3f, 0, 0));
                yield return new WaitForSeconds(0.2f);
                _mat.SetColor("_ColorGlitter", Color.black);
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}