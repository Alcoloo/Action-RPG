using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : Enemy {

    // Use this for initialization
    protected BehaviorTree tree;

    void Start () {
        tree = GetComponent<BehaviorTree>();
        Init();
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
            SetModeChargePlayer();
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
