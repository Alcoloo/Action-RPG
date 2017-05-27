
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Rpg.Controller;
using System.Collections;
using Rpg;

public class BasicAttack : Action
{
    private Enemy _enemyScript;
    private bool _isOnRange = false;
    private bool _isAttacked = false;
    private bool _isOnAttack = false;
    private float _animationTime = 1f;
    private float _currentAnimationTime = 0f;
    private float _cooldown = 2f;

    public override void OnStart()
    {
        _isOnRange = false;
        _isAttacked = false;
        _isOnAttack = false;
        _enemyScript = GetComponent<Enemy>();
        if (_enemyScript.isOnAttackRange()) _isOnRange = true;
        else
        {
            GetComponent<WeaponController>().activateWeaponAttack(Rpg.HAND.right, false);
            _isOnRange = false;
        }
        base.OnStart();
    }

    //couroutine d'attack 

    public override TaskStatus OnUpdate()
    {
        if (!_isOnRange) return TaskStatus.Failure;
        
        else
        {
            if (_isAttacked)
            {
                _isAttacked = false;
                return TaskStatus.Success;
            }
            if (!_isOnAttack)
            {
                _isOnAttack = true;
                _enemyScript.ChangeAnimationState("attack");

                StartCoroutine(attackCoroutine());
            }
            return TaskStatus.Running;
        }
    }


    public IEnumerator attackCoroutine()
    {
        while(_currentAnimationTime < _animationTime)
        {
            _currentAnimationTime += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        _currentAnimationTime = 0;
        _isOnAttack = false;
        _isAttacked = true;

    }
}

