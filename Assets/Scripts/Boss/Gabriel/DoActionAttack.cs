using BehaviorDesigner.Runtime.Tasks;
using Rpg;
using Rpg.Characters;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Boss.Gabriel
{
    
    /// <summary>
    /// 
    /// </summary>
    public class DoActionAttack : Action
    {
        public float angle;
        public float radius;
        public int _damage;

        public override TaskStatus OnUpdate()
        {
            Vector3 directionToTarget = (Player.instance.transform.position - transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, Player.instance.transform.position);
            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2 && distanceToTarget < radius)
            {
                Player.instance.TakeDamage(_damage);
                return TaskStatus.Success;
            }
            else return TaskStatus.Success;
        }


    }
}