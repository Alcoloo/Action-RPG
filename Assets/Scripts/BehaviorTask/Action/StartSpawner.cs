using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Rpg;

namespace rpg
{
    public class StartSpawner : Action
    {
        private float startSpawnerTime = 0f;

        private float startSummon = 0f;
        public float SummonTime = 2f;

        public override void OnStart()
	    {
            startSummon = CustomTimer.instance.elapsedTime;
        }

	    public override TaskStatus OnUpdate()
	    {
            if (CustomTimer.instance.isTime(startSummon, SummonTime))
            {
                if (startSpawnerTime == 0)
                {
                    startSpawnerTime = CustomTimer.instance.elapsedTime;
                    EnemyManager.instance.startSpawners();
                    return TaskStatus.Success;
                }
                else if (CustomTimer.instance.isTime(startSpawnerTime, EnemyManager.instance.spawnerCooldown) && EnemyManager.instance.IsNotTooMuchPopCorn())
                {
                    startSpawnerTime = CustomTimer.instance.elapsedTime;
                    EnemyManager.instance.startSpawners();
                    return TaskStatus.Success;
                }
                else return TaskStatus.Failure;
            }
            else return TaskStatus.Running;
	    }
    }
}