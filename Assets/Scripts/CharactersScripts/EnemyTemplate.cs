using UnityEngine;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.CharactersScripts
{
    [System.Serializable]
    public class EnemyTemplate
    {
        public string EnemyName;

        [Header("health and Dmg")]
        [SerializeField]
        protected int m_health;
        [SerializeField]
        protected int m_armor;
        [Header("Range")]
        [SerializeField]
        protected float m_detectionRange;
        [SerializeField]
        protected float m_attackDistance;
        [Header("Speed")]
        [SerializeField]
        protected float m_rotationSpeed;
        [SerializeField]
        protected float m_attackMovementSpeed;
        [SerializeField]
        protected float m_speed;
        [Header("Timer")]
        [SerializeField]
        protected float m_hurtDuration;
        [SerializeField]
        protected float m_attackDuration;

        #region Getteur
        public int health
        {
            get { return m_health; }
        }

        public int armor
        {
            get { return m_armor; }
        }

        public float detectionRange
        {
            get { return m_detectionRange; }
        }

        public float attackDistance
        {
            get { return m_attackDistance; }
        }

        public float rotationSpeed
        {
            get { return m_rotationSpeed; }
        }

        public float attackMovementSpeed
        {
            get { return m_attackMovementSpeed; }
        }

        public float speed
        {
            get { return m_speed; }
        }

        public float hurtDuration
        {
            get { return m_hurtDuration; }
        }

        public float attackDuration
        {
            get { return m_attackDuration; }
        }
        #endregion

        #region Constructeur
     /*   public EnemyTemplate ()
        {

        }*/

        public EnemyTemplate (EnemyTemplate copy)
        {
            m_health = copy.health;
            m_armor = copy.armor;
            m_detectionRange = copy.detectionRange;
            m_attackDistance = copy.attackDistance;
            m_rotationSpeed = copy.rotationSpeed;
            m_attackMovementSpeed = copy.attackMovementSpeed;
            m_speed = copy.speed;
            m_hurtDuration = copy.hurtDuration;
            m_attackDuration = copy.m_attackDuration;

        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>

}