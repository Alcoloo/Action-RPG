using Assets.Scripts.CharactersScripts;
using BehaviorDesigner.Runtime;
using Rpg;
using Rpg.Characters;
using Rpg.Controller;
using Rpg.GraphicElement.Weapons;
using Rpg.Manager;
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

        /* A VOIR SI ON GARDE AVEC BALANCING MANAGER
        #region variable enemy parametrable
        public float detectionRange;
        public float attackDistance;
        public float rotationSpeed;
        public float attackMovementSpeed;
        public float speed;
        protected float m_hurtDuration;
        protected float m_attackDuration;
        #endregion
           
         */

        #region variable class

        public int m_currentHealth { get; protected set; }
        protected System.Action DoAction;
        protected NavMeshAgent m_nav;
        protected float m_playerDistance;

        [SerializeField]
        protected EnemyTemplate m_template;
        protected float m_startTime;
        protected GameObject m_healthBar;
        protected HealthRotation m_healthBarScript;
        protected Weapon m_weapon;
        protected Animator m_anim;
        protected string[] m_animNames = { "run", "walk", "attack", "canalize", "hurt", "charge", "die", "shoot" };
        [SerializeField]
        protected GameObject m_prefab;
        public float health { get; private set; }

        #endregion

        #region Du constructeur à l'Init

        //TODO Weapon n'est pas set de base

        protected virtual void Awake()
        {
            if (BalancingManager.manager != null && BalancingManager.manager.isReady) InitAfterBalancing();
            else BalancingManager.manager.onReady.AddListener(InitAfterBalancing);
            player = GameObject.FindGameObjectWithTag("Player").transform;
            DoAction = DoActionVoid;
        }

        protected virtual void InitAfterBalancing()
        {
            BalancingManager.manager.onReady.RemoveListener(InitAfterBalancing);
            m_template = new EnemyTemplate(BalancingManager.manager.getEnemy(m_prefab.name));
            m_currentHealth = m_template.health;
       

            if (transform.FindChild("HealthComponent"))
            {
                m_healthBar = transform.FindChild("HealthComponent").gameObject;
                m_healthBarScript = m_healthBar.GetComponent<HealthRotation>();
                m_healthBar.SetActive(false);
            }
            m_nav = GetComponent<NavMeshAgent>();
            if (GetComponent<Weapon>()) m_weapon = GetComponent<Weapon>();
            m_anim = GetComponentInChildren<Animator>();
            health = m_template.health;
        }

        // Use this for initialization
        public virtual void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            if (!GetComponent<CapsuleCollider>().enabled) GetComponent<CapsuleCollider>().enabled = true;
        }

        public void Init()
        {

        }

        public void FixedUpdate()
        {
            m_playerDistance = Vector3.Distance(transform.position, player.position);
            DoAction();
        }

        #endregion

        public virtual void TakeDamage(int damage)
        {
            Debug.Log("Enemy Took Damage");
            if (m_currentHealth - damage > 0) m_currentHealth -= damage;
            else m_currentHealth = 0;
            if (m_currentHealth > 0) SetModeHurt();
            else SetModeDie();
            if (m_healthBar != null) UpdateHealthBar();
        }

        #region State Machine

        private void HitSomething(GameObject player)
        {
            Debug.Log("Enemy hit Player");
            Player.instance.TakeDamage(m_template.damage);
        }

        public virtual void UpdateHealthBar()
        {
            if (!m_healthBar.activeInHierarchy) m_healthBar.SetActive(true);
            m_healthBarScript.changeLife((float)m_currentHealth / (float)m_template.health);
        }

        protected virtual void SetModeVoid() { DoAction = DoActionVoid; }

        protected virtual void DoActionVoid() { }

        protected virtual void SetModeHurt()
        {
            ChangeAnimationState("hurt");
        }

        protected virtual void SetModeDie()
        {
            GetComponent<BehaviorTree>().DisableBehavior();
            GetComponent<CapsuleCollider>().enabled = false;
            m_startTime = CustomTimer.manager.elapsedTime;
            DoAction = DoActionDie;
        }

        protected virtual void DoActionDie()
        {
            if (CustomTimer.manager.isTime(m_startTime, m_anim.GetCurrentAnimatorStateInfo(0).length / 3)) Destroy(m_healthBar);
            if (CustomTimer.manager.isTime(m_startTime, m_anim.GetCurrentAnimatorStateInfo(0).length)) gameObject.SetActive(false);
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

        #region animation

        private void ResetAllAnimStates()
        {
            if (m_anim != null)
            {
                for (int i = 0; i < m_animNames.Length; i++)
                {
                    if (m_anim.GetBool(m_animNames[i])) m_anim.SetBool(m_animNames[i], false);
                }

            }
        }

        public void ChangeAnimationState(string pState)
        {
            ResetAllAnimStates();
            if (m_anim != null) if (!m_anim.GetBool(pState)) m_anim.SetBool(pState, true);
        }

        #endregion

        #region test

        public bool isDetected() { return (m_playerDistance < m_template.detectionRange); }

        public bool isOnAttackRange() { return (m_playerDistance < m_template.attackDistance); }

        #endregion

        #region attackCoolDown

        public void SetStartTime()
        {
            m_startTime = CustomTimer.manager.elapsedTime;
        }

        public float GetStartTime()
        {
            if (m_startTime != 0f) return m_startTime;
            else return 0f;
        }
        #endregion
    }
}