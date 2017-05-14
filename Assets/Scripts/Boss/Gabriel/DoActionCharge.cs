using BehaviorDesigner.Runtime.Tasks;
using Rpg;
using UnityEngine;

namespace Assets.Scripts.Boss.Gabriel
{

    /// <summary>
    /// 
    /// </summary>
    public class DoActionCharge : Action
    {
        delegate void LaunchAction();
        private LaunchAction DoAction;

        public float chargePowerTime;
        public float speed;

        protected void Start()
        {
            SetModeChargePower();
        }

        protected void Update()
        {
            DoAction();
        }

        private void SetModeChargePower()
        {
            DoAction = DoActionChargePower;
        }

        private void SetModeReleasePower()
        {
            DoAction = DoActionReleasePower;
        }

        private void SetModeRush()
        {
            DoAction = DoActionRush;
        }

        private void DoActionChargePower()
        {
            chargePowerTime -= Time.deltaTime;
            Debug.Log("charge : " + chargePowerTime);
            if (chargePowerTime <= 0.0f) SetModeReleasePower();
        }

        private void DoActionReleasePower()
        {
            Debug.Log("ENCULE ET ETOURDIT LE JOUEUR");
            SetModeRush();
        }

        private void DoActionRush()
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.instance.transform.position - Vector3.forward, Time.deltaTime * speed);
        }
    }
}