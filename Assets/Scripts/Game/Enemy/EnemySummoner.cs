using BehaviorDesigner.Runtime;
using Rpg.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySummoner : Enemy {

    // Use this for initialization
    protected BehaviorTree tree  ;
    protected int fireRate = 120;
    protected int counter = 0;
	void Start () {
        tree = GetComponent<BehaviorTree>();
        weapon = GetComponent<WeaponController>();
        init();
        SetModePatrol();
	}

    protected void SetModePatrol()
    {
        //nav.Stop();
        tree.EnableBehavior();
        nav.speed = speed;
        DoAction = DoActionPatrol;
    }

    protected void DoActionPatrol()
    {
        if (isDetected())
        {
            tree.DisableBehavior();
            SetModeSummon();
        }
    }

    protected void SetModeSummon()
    {
        EnemyManager.instance.startSpawners();
        DoAction = DoActionSummon;
    }

    protected void DoActionSummon()
    {
        SetModeFight();
    }

    protected void SetModeFight()
    {
        counter = 0;
        DoAction = DoActionFight;
    }

    protected void DoActionFight()
    {
        counter++;
        if(player != null) transform.LookAt(player);
        if (isOnAttackRange()) SetModeFear();
        if(counter % fireRate == 0)
        {
            createShoot();
        }
        if (counter >= 1000 && EnemyManager.instance.getPopCornCount() < 5) ;
    }

    protected void SetModeFear()
    {
        DoAction = DoActionFear;
    }

    protected void DoActionFear()
    {

    }
    // Update is called once per frame
    void Update () {
		
	}
}
