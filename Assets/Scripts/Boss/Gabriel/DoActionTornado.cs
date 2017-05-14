using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using UnityEngine;

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

        protected void Update()
        {
            chargeWings -= Time.deltaTime;
            Debug.Log("charge : " + chargeWings);
            if (chargeWings <= 0.0f)
            {
                CreateTornado();
                chargeWings = startChargeWings;
            }
        }
        
        private void CreateTornado()
        {
            GameObject tornadoPrefab = PoolingManager.instance.getFromPool("Tornado");
            tornadoPrefab.transform.SetParent(gameObject.transform, false);
            tornadoPrefab.transform.position = gameObject.transform.position;
        }
    }
}