using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

namespace Rpg
{

    /// <summary>
    /// 
    /// </summary>
    public class ChasePlayer : Action
    {
        private Enemy enemyScript;
        private Transform target;
        private NavMeshAgent nav;

        public override void OnStart()
        {
            base.OnStart();
            enemyScript = GetComponent<Enemy>();
            target = enemyScript.player;
            nav = GetComponent<NavMeshAgent>();
        }

        public override TaskStatus OnUpdate()
        {
            if (enemyScript.isOnAttackRange())
            {
                nav.Stop();
                return TaskStatus.Success;
            }
            enemyScript.playerAnimationTest();
            transform.LookAt(target);
            nav.destination = target.position;
            nav.Resume();

            return TaskStatus.Running;
        }
    }
}