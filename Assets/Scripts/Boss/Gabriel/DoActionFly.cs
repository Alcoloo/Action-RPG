using UnityEngine;

namespace Assets.Scripts.Boss.Gabriel
{

    /// <summary>
    /// 
    /// </summary>
    public class DoActionFly : MonoBehaviour
    {
        public float speed;
        public float chargePower;
        private float startChargePower;

        delegate void LaunchAction();
        private LaunchAction DoAction;

        protected void Start()
        {
            startChargePower = chargePower;
            SetModFlyAndCharge();
        }

        protected void Update()
        {
            DoAction();
        }

        private void SetModFlyAndCharge()
        {
            DoAction = DoActionFlyAndCharge;
        }

        private void SetModeReleasePower()
        {
            DoAction = DoActionReleasePower;
        }

        private void DoActionFlyAndCharge()
        {
            transform.Translate(Vector3.up * speed);

            chargePower -= Time.deltaTime;
            Debug.Log("charge : " + chargePower);
            if (chargePower <= 0.0f)
            {
                chargePower = startChargePower;
                SetModeReleasePower();
            }
        }

        private void DoActionReleasePower()
        {
            Debug.Log("ECNULE LE JOUEUR !");
        }
    }
}