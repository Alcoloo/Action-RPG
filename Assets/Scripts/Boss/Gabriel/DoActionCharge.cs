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
        private Enemy _enemyScript;

        public override void OnStart()
        {
            _enemyScript = GetComponent<Enemy>();
            startPowerTime = CustomTimer.manager.elapsedTime;
            hasReleasedPower = false;
            startHP = _enemyScript.health;
            currentHP = _enemyScript.m_currentHealth;
            _enemyScript.ChangeAnimationState("canalize");
        }

        public override TaskStatus OnUpdate()
        {
            if(_enemyScript != null) currentHP = _enemyScript.m_currentHealth;

            if (!hasReleasedPower)
            {
                if (CustomTimer.manager.isTime(startPowerTime, chargePowerTime))
                {
                    Debug.Log("released Power");
                    if(gameObject.name == "Gabriel") Player.instance.GetComponent<RPGCharacterController>().rpgCharacterState = RPGCharacterState.CINEMATIC;
                    Player.instance.TakeDamage(_damage);
                    hasReleasedPower = true;
                    _enemyScript.ChangeAnimationState("run");
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
                    _enemyScript.ChangeAnimationState("attack");
                    return TaskStatus.Success;
                }
                else return TaskStatus.Running;
            }
            
        }
        
    }
}