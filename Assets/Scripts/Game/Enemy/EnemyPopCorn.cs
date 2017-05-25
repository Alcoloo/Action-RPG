using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Formations.Tasks;
using BehaviorDesigner.Runtime.Tactical.Tasks;
using Rpg.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPopCorn : Enemy {

    protected BehaviorTree tree;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    // Use this for initialization
    public override void Start() {
        
        nav.speed = speed;
        //GetComponent<BehaviorTree>().FindTask<Skirmisher>().targetTransform = player.transform;
        //SetModeChargePlayer();
        GetComponent<WeaponController>().activateWeaponAttack(Rpg.HANDKIND.right, true);
	}



}
