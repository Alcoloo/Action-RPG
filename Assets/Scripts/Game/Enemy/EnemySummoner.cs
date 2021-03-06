﻿using BehaviorDesigner.Runtime;
using Rpg.Controller;
using Rpg.GraphicElement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rpg;

namespace Rpg {
    public class EnemySummoner : Enemy {

        // Use this for initialization
        protected BehaviorTree tree;
        protected int fireRate = 120;
        protected int counter = 0;

       public override void Start() {
            tree = GetComponent<BehaviorTree>();
            weapon = GetComponent<Weapon>(); //TODO Corriger avec la migration de WeaponController dans Weapon
            Init();
            SetModePatrol();
        }

        protected void SetModePatrol()
        {
            //nav.Stop();
            tree.EnableBehavior();
            nav.speed = template.speed;
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
            EnemyManager.manager.startSpawners();
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
            if (player != null) transform.LookAt(player);
            if (isOnAttackRange()) SetModeFear();
            if (counter % fireRate == 0)
            {
                createShoot();
            }
            if (counter >= 1000 && EnemyManager.manager.getPopCornCount() < 5) ;
        }

        protected void SetModeFear()
        {
            DoAction = DoActionFear;
        }

        protected void DoActionFear()
        {

        }

        public void createShoot()
        {
            //TODO A voir après la migration de weaponController en weapon
            //weapon.curentShooter.Shoot(transform.rotation, transform.position + transform.forward);
        }
        // Update is called once per frame
        void Update() {

        }
    }
}