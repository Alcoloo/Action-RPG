using BehaviorDesigner.Runtime.Tasks;
using Rpg;
using Rpg.Characters;
using UnityEngine;

namespace Assets.Scripts.Boss.Gabriel
{

    /// <summary>
    /// 
    /// </summary>
    public class DoActionFly : Action
    {
        public float speed;
        public float chargePower;
        public int _damage;
        public float _shieldLife;

        private float startChargePower;
        private float startYPosition;
        private Caracteristic cara;
        private float currentHP;
        private float startHP;

        public override void OnStart()
        {
            startChargePower = CustomTimer.instance.elapsedTime;
            startYPosition = transform.position.y;
            cara = GetComponent<Caracteristic>();
            startHP = cara.pv;
            currentHP = startHP;
        }

        public override TaskStatus OnUpdate()
        {
            currentHP = cara.pv;
            transform.Translate(Vector3.up * speed);
            if (CustomTimer.instance.isTime(startChargePower, chargePower))
            {
                if (currentHP >= startHP - _shieldLife) Player.instance.GetComponent<Caracteristic>().TakeDamage(_damage, KIND.none);
                transform.position = new Vector3(transform.position.x, startYPosition, transform.position.z);
                return TaskStatus.Success;
            }
            else return TaskStatus.Running;
        }
        
    }
}