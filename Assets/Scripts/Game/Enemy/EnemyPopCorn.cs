using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Formations.Tasks;
using BehaviorDesigner.Runtime.Tactical.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPopCorn : Enemy {

    protected BehaviorTree tree;

    // Use this for initialization
    void Start () {
       // init();
        //tree = GetComponent<BehaviorTree>();
        //tree.EnableBehavior();
        nav.speed = speed;
        //GetComponent<BehaviorTree>().FindTask<Skirmisher>().targetTransform = player.transform;
        //SetModeChargePlayer();
	}



}
