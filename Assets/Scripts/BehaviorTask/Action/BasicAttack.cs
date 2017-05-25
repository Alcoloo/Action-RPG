
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Rpg.Controller;
using System.Collections;
using Rpg;

public class BasicAttack : Action
{
    private Enemy enemyScript;
    private bool isOnRange = false;
    private bool isAttacked = false;
    private bool isOnAttack = false;
    private float animationTime = 1f;
    private float currentAnimationTime = 0f;
    private float cooldown = 2f;

    public override void OnStart()
    {
        isOnRange = false;
        isAttacked = false;
        isOnAttack = false;
        enemyScript = GetComponent<Enemy>();
        if (enemyScript.isOnAttackRange()) isOnRange = true;
        else
        {
            GetComponent<WeaponController>().activateWeaponAttack(Rpg.HANDKIND.right, false);
            isOnRange = false;
        }
        base.OnStart();
    }

    //couroutine d'attack 

    public override TaskStatus OnUpdate()
    {
        if (!isOnRange) return TaskStatus.Failure;
        
        else
        {
            if (isAttacked)
            {
                isAttacked = false;
                return TaskStatus.Success;
            }
            if (!isOnAttack)
            {
                isOnAttack = true;
                GetComponent<WeaponController>().activateWeaponAttack(Rpg.HANDKIND.right, true);
                StartCoroutine(attackCoroutine());
            }
            return TaskStatus.Running;
        }
    }


    public IEnumerator attackCoroutine()
    {
        while(currentAnimationTime < animationTime)
        {
            currentAnimationTime += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        currentAnimationTime = 0;
        isOnAttack = false;
        isAttacked = true;
        GetComponent<WeaponController>().activateWeaponAttack(Rpg.HANDKIND.right, false);
    }
}

