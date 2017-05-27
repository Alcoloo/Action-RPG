using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rpg.Controller;
using Rpg.GraphicElement;
using Rpg;


public class EnemyTank : Enemy {

    // Use this for initialization
    protected BehaviorTree tree;

    void Start () {
        tree = GetComponent<BehaviorTree>();
        Init();
	}
    // Update is called once per frame
    void Update () {
		
	}
}
