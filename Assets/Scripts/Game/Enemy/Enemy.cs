using Assets.Scripts.CharactersScripts;
using Rpg;
using Rpg.Characters;
using Rpg.Controller;
using Rpg.GraphicElement.Weapons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Rpg
{
    public class Enemy : MonoBehaviour
    {

        #region variable player
        public Transform player;
        #endregion


        #region variable class
        protected int currentHealth;
        protected System.Action DoAction;
        protected Rigidbody myRigidbody;
        protected MeshRenderer myMesh;
        protected NavMeshAgent nav;
        protected float playerDistance;
        [SerializeField]
        protected EnemyTemplate template;
        protected float startTime;
        protected GameObject healthBar;
        protected HealthRotation healthBarScript;
        protected Weapon weapon;
        #endregion

        #region Du constructeur à l'Init
        //TODO Weapon n'est pas set de base
        public Enemy()
        {
            template = new EnemyTemplate(EnemiesStat.instance.getEnemy("enemy"));
            currentHealth = template.health;
            //pour eviter le "find" le manager envera le gameObject Player au enemy
            player = GameObject.FindGameObjectWithTag("Player").transform;
            myRigidbody = GetComponent<Rigidbody>();
            nav = GetComponent<NavMeshAgent>();
            myMesh = GetComponent<MeshRenderer>();
            healthBar = transform.FindChild("HealthComponent").gameObject;
            healthBarScript = healthBar.GetComponent<HealthRotation>();
            if (GetComponent<Weapon>()) weapon = GetComponent<Weapon>();
            DoAction = DoActionVoid;

        }

        protected virtual void Awake()
        {
            healthBar.SetActive(false);
        }


        // Use this for initialization
        public virtual void Start()
        {
        }

        public void Init()
        {

        }
        #endregion

        public void TakeDamage(int damage)
        {
            if (currentHealth - damage > 0)
                currentHealth -= damage;
            else
                currentHealth = 0;

            UpdateHealthBar();

            if (currentHealth > 0)
                SetModeHurt();
            else
                SetModeDie();
        }

        public void UpdateHealthBar()
        {
            healthBarScript.changeLife((float)currentHealth / (float)template.health);
        }

        #region ajout des componentes
        protected Caracteristic getCaracComponent(GameObject pEnemy)
        {
            if (pEnemy.GetComponent<Caracteristic>() == null) pEnemy.AddComponent<Caracteristic>();
            return pEnemy.GetComponent<Caracteristic>();
        }
        #endregion

        protected virtual void FixedUpdate()
        {
            playerDistance = Vector3.Distance(player.position, transform.position);
            //DoAction();
        }

        #region State Machine

        protected void SetModeVoid() { DoAction = DoActionVoid; }

        protected void DoActionVoid() { }

        protected void SetModeChargePlayer()
        {
            myMesh.material.color = Color.red;
            DoAction = DoActionChargePlayer;
        }

        protected void DoActionChargePlayer()
        {
            transform.LookAt(player);
            nav.destination = player.position;
            nav.speed = template.attackMovementSpeed;
            nav.Resume();
            if (playerDistance < template.attackDistance) SetModeBasicAttack();
        }

        protected void SetModeBasicAttack()
        {
            nav.Stop();
            myMesh.material.color = Color.black;
            startTime = CustomTimer.manager.elapsedTime;
            DoAction = DoActionBasicAttack;
        }

        protected void DoActionBasicAttack()
        {
            if (CustomTimer.manager.isTime(startTime, template.hurtDuration)) SetModeChargePlayer();
        }

        protected virtual void SetModeHurt()
        {
            startTime = CustomTimer.manager.elapsedTime;
            myMesh.material.color = Color.yellow;
            DoAction = DoActionHurt;
        }

        protected virtual void DoActionHurt()
        {
            if (CustomTimer.manager.isTime(startTime, template.hurtDuration)) SetModeChargePlayer();
        }

        protected void SetModeDie()
        {
            DoAction = DoActionDie;
        }

        protected void DoActionDie()
        {
            Destroy();
            //si l'animation de mort et fini destroy()
        }



        public void lookPlayer()
        {
            transform.LookAt(player);
        }

        protected virtual void Destroy()
        {
            SetModeVoid();
            gameObject.SetActive(false);
        }
        #endregion

        public void playerAnimationTest()
        {
            int run = Animator.StringToHash("run");
            GetComponentInChildren<Animator>().SetTrigger(run);
        }

        #region test
        public bool isDetected() { return (playerDistance < template.detectionRange); }

        public bool isOnAttackRange() { return (playerDistance < template.attackDistance); }
        #endregion

        #region attackCoolDown
        public void SetStartTime()
        {
            startTime = CustomTimer.manager.elapsedTime;
        }

        public float GetStartTime()
        {
            if (startTime != 0f) return startTime;
            else return 0f;
        }
        #endregion
    }
}