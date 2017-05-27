using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Rpg;

namespace Rpg {

    /// <summary>
    /// 
    /// </summary>
    public class StartSpawner : Action
    {
        private float startSpawnerTime = 0f;
        private Enemy enemyScript;
        private float startSummon = 0f;
        public float SummonTime = 2f;

        public override void OnStart()
	    {
            startSummon = CustomTimer.manager.elapsedTime;
            enemyScript = GetComponent<Enemy>();
        }

	    public override TaskStatus OnUpdate()
	    {
            if (CustomTimer.manager.isTime(startSummon, SummonTime))
            {
                if (startSpawnerTime == 0)
                {
                    startSpawnerTime = CustomTimer.manager.elapsedTime;
                    EnemyManager.manager.startSpawners();
                    enemyScript.ChangeAnimationState("canalize");
                    return TaskStatus.Success;
                }
                else if (CustomTimer.manager.isTime(startSpawnerTime, EnemyManager.manager.spawnerCooldown) && EnemyManager.manager.IsNotTooMuchPopCorn())
                {
                    startSpawnerTime = CustomTimer.manager.elapsedTime;
                    EnemyManager.manager.startSpawners();
                    return TaskStatus.Success;
                }
                else return TaskStatus.Failure;
            }
            else return TaskStatus.Running;
	    }
    }
}