using BehaviorDesigner.Runtime.Tasks;
using Rpg;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Boss.Gabriel
{

    /// <summary>
    /// 
    /// </summary>
    public class DoActionRush : Action
    {
        public float chargeRush;
        private float startChargeRush;
        private bool isRushing = false;
        private float _minDistance = 0.5f;
        private int _rushNumber = 0;
        public int _rushMax = 6;
        public float speed;

        delegate void LauchAction();
        private LauchAction DoAction;

        protected void Start()
        {
            startChargeRush = chargeRush;
            SetModeCharge();
        }

        protected void Update()
        {
            DoAction();
        }

        private void DoActionCharge()
        {
            chargeRush -= Time.deltaTime;
            Debug.Log("charge : " + chargeRush);
            if (chargeRush <= 0.0f)
            {
                chargeRush = startChargeRush;
                _rushNumber++;
                SetModeRush();
            }
        }

        private void SetModeRush()
        {
            DoAction = DoActionRushOnPlayer;
        }

        private void SetModeCharge()
        {
            DoAction = DoActionCharge;
        }

        private void DoActionRushOnPlayer()
        {
            Vector3 startPos = transform.position;
            Vector3 endPos = Player.instance.transform.position;
            
            transform.position = Vector3.MoveTowards(startPos, endPos, Time.deltaTime * speed);

            if (Vector3.Distance(transform.position,endPos) < _minDistance && _rushNumber < _rushMax)
            {
                Debug.Log("charge");
                SetModeCharge();
            }
           
        }
    }
}