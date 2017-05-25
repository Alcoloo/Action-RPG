using BehaviorDesigner.Runtime.Tasks;
using Rpg;
using Rpg.Characters;
using UnityEngine;

namespace Assets.Scripts.Boss.Gabriel
{

    /// <summary>
    /// 
    /// </summary>
    public class DoActionCharge : Action
    {
        public float chargePowerTime;      
        public float speed;
        public float _minDistance;
        public float _shieldLife;
        public int _damage;

        private bool hasReleasedPower;
        private float startPowerTime;
        private float startHP;
        private float currentHP;
        private Caracteristic cara;

        public override void OnStart()
        {
            startPowerTime = CustomTimer.manager.elapsedTime;
            hasReleasedPower = false;
            cara = GetComponent<Caracteristic>();
            startHP = cara.pv;
            currentHP = startHP;
        }

        public override TaskStatus OnUpdate()
        {
            currentHP = cara.pv;
            if(!hasReleasedPower)
            {
                if (CustomTimer.manager.isTime(startPowerTime, chargePowerTime))
                {
                    Debug.Log("released Power");
                    if(gameObject.name == "Gabriel") Player.instance.GetComponent<RPGCharacterController>().rpgCharacterState = RPGCharacterState.CINEMATIC;
                    Player.instance.GetComponent<Caracteristic>().TakeDamage(_damage, KIND.none);
                    hasReleasedPower = true;
                    return TaskStatus.Running;
                }
                else if (currentHP <= startHP - _shieldLife)
                {
                    Debug.Log("stop charge");
                    return TaskStatus.Success;
                }
                else return TaskStatus.Running;
            }
            else
            {
                transform.LookAt(Player.instance.transform);
                transform.position = Vector3.MoveTowards(transform.position, Player.instance.transform.position + Vector3.up, Time.deltaTime * speed);
                if (Vector3.Distance(transform.position, Player.instance.transform.position) <= _minDistance)
                {
                    Player.instance.GetComponent<RPGCharacterController>().rpgCharacterState = RPGCharacterState.DEFAULT;
                    hasReleasedPower = false;
                    return TaskStatus.Success;
                }
                else return TaskStatus.Running;
            }
            
        }
        
    }
}