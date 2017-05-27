using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Rpg;

public class SimpleShoot : Action
{
    private EnemySummoner enemyScript;

    private bool isShooting = false;
    private float startTime = 0;

    public float fireRate = 4f;
   

    public override void OnStart()
	{
        enemyScript = GetComponent<EnemySummoner>();
    }

	public override TaskStatus OnUpdate()
	{
        if(!isShooting)
        {
            enemyScript.lookPlayer();
            enemyScript.CreateShoot();
            isShooting = true;
            startTime = CustomTimer.manager.elapsedTime;
            enemyScript.ChangeAnimationState("shoot");
            return TaskStatus.Running;
        }
        else
        {
            if (CustomTimer.manager.isTime(startTime, fireRate))
            {
                isShooting = false;
                return TaskStatus.Success;
            }
            return TaskStatus.Running;
        }
	}
}