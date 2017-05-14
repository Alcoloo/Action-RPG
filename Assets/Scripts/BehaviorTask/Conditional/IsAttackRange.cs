using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class IsAttackRange : Conditional
{
    private GameObject enemy;
    private Enemy enemyScript;

    public override void OnStart()
    {
        base.OnStart();
        enemy = transform.gameObject;
        enemyScript = enemy.GetComponent<Enemy>();
    }

    public override TaskStatus OnUpdate()
    {
        if (enemyScript.isOnAttackRange()) return TaskStatus.Success;
        return TaskStatus.Failure;
    }
}