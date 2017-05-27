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
        private Enemy _enemyScript;
        private Transform _target;
        private NavMeshAgent _nav;

        public override void OnStart()
        {
            base.OnStart();
            _enemyScript = GetComponent<Enemy>();
            _target = _enemyScript.player;
            _nav = GetComponent<NavMeshAgent>();
        }

        public override TaskStatus OnUpdate()
        {
            if (_enemyScript.isOnAttackRange())
            {
                _nav.Stop();
                return TaskStatus.Success;
            }
            _enemyScript.ChangeAnimationState("run");
            transform.LookAt(_target);
            _nav.destination = _target.position;
            _nav.Resume();

            return TaskStatus.Running;
        }
    }
}