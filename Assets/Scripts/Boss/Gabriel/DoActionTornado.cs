using BehaviorDesigner.Runtime.Tasks;
using Rpg;
using System.Collections;
using UnityEngine;
using Rpg;

namespace Assets.Scripts.Boss.Gabriel
{

    /// <summary>
    /// 
    /// </summary>
    public class DoActionTornado : Action
    {
        public GameObject tornado;
        public float chargeWings;
        private float startChargeWings;

        protected void Start()
        {
            startChargeWings = chargeWings;
        }

        public override void OnStart()
        {
            startChargeWings = CustomTimer.instance.elapsedTime;
        }

        public override TaskStatus OnUpdate()
        {
            if (CustomTimer.instance.isTime(startChargeWings, chargeWings))
            {
                transform.LookAt(Player.instance.transform);
                CreateTornado();
                return TaskStatus.Success;
            }
            else return TaskStatus.Running;
        }
        
        private void CreateTornado()
        {
            GameObject tornadoPrefab = PoolingManager.instance.getFromPool("Tornado");
            tornadoPrefab.SetActive(true);
            tornadoPrefab.transform.SetParent(gameObject.transform, false);
            tornadoPrefab.transform.position = gameObject.transform.position + Vector3.forward;
        }
    }
}