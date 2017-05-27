using Assets.Scripts.CharactersScripts;
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
        #endregion        /* A VOIR SI ON GARDE AVEC BALANCING MANAGER
        #region variable enemy parametrable
        public float detectionRange;
        public float attackDistance;
        public float rotationSpeed;
        public float attackMovementSpeed;
        public float speed;
        protected float m_hurtDuration;
        protected float m_attackDuration;
        #endregion         */

        #region variable class
        protected int m_currentHealth;
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
        #endregion
        #region Du constructeur à l'Init
        //TODO Weapon n'est pas set de base
        protected virtual void Awake()
        {
            if (BalancingManager.manager.isReady) InitAfterBalancing();
            else BalancingManager.manager.onReady.AddListener(InitAfterBalancing);
        }
        protected virtual void InitAfterBalancing()
        {
            BalancingManager.manager.onReady.RemoveListener(InitAfterBalancing);
            m_template = new EnemyTemplate(BalancingManager.manager.getEnemy(m_prefab.name));
            m_currentHealth = m_template.health;
            player = GameObject.FindGameObjectWithTag("Player").transform;
            m_healthBar = transform.FindChild("HealthComponent").gameObject;
            m_healthBarScript = m_healthBar.GetComponent<HealthRotation>();
            m_nav = GetComponent<NavMeshAgent>();
            if (GetComponent<Weapon>()) m_weapon = GetComponent<Weapon>();
            //pour eviter le "find" le manager envera le gameObject Player au enemy
            m_healthBar.SetActive(false);
            m_anim = GetComponentInChildren<Animator>();
            DoAction = DoActionVoid;
        }
        // Use this for initialization
        public virtual void Start()
        {            player = EnemyManager.manager.player;
        }
        public void Init()
        {
        }


        #endregion
        public void TakeDamage(int damage)
        {
            Debug.Log("Enemy Took Damage");
            if (m_currentHealth - damage > 0)
                m_currentHealth -= damage;
            else
                m_currentHealth = 0;

            UpdateHealthBar();
            if (m_currentHealth > 0)
                SetModeHurt();
            else
                SetModeDie();
        }
        #region State Machine
        private void HitSomething(GameObject player)
        {
            Debug.Log("Enemy hit Player");
            Player.instance.TakeDamage(m_template.damage);
        }
        public void UpdateHealthBar()
        {
            m_healthBarScript.changeLife((float)m_currentHealth / (float)m_template.health);
        }
        protected void SetModeVoid() { DoAction = DoActionVoid; }
        protected void DoActionVoid() { }
        protected virtual void SetModeHurt()
        {
            ChangeAnimationState("hurt");
        }
        protected void SetModeDie() { gameObject.SetActive(false); }
        protected void DoActionDie()
        {
            //Destroy();
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
        {            m_startTime = CustomTimer.manager.elapsedTime;
        }
        public float GetStartTime()
        {
            if (m_startTime != 0f) return m_startTime;
            else return 0f;
        }        #endregion
    }
}
