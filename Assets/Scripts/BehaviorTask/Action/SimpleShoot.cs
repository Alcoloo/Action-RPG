using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Assets.Scripts.Utils.Timer;

public class SimpleShoot : Action
{
    private Enemy enemyScript;

    private bool isShooting = false;
    private float startTime = 0;

    public float fireRate = 4f;
   

    public override void OnStart()
	{
        enemyScript = GetComponent<Enemy>();
    }

	public override TaskStatus OnUpdate()
	{
        if(!isShooting)
        {
            enemyScript.lookPlayer();
            enemyScript.createShoot();
            isShooting = true;
            startTime = CustomTimer.instance.elapsedTime;
            return TaskStatus.Running;
        }
        else
        {
            if (CustomTimer.instance.isTime(startTime, fireRate))
            {
                isShooting = false;
                return TaskStatus.Success;
            }
            return TaskStatus.Running;
        }
	}
}