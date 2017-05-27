using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Formations.Tasks;
using BehaviorDesigner.Runtime.Tactical.Tasks;
using Rpg;
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
        

        m_nav.speed = m_template.speed;
        GetComponent<WeaponController>().activateWeaponAttack(Rpg.HAND.right, true);
	}



}
