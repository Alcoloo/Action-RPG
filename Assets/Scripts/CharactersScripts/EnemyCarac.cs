using UnityEngine;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.CharactersScripts
{
    [System.Serializable]
    public class EnemySpe
    {
        public string EnemyName;

        [Header("Life and Dmg")]
        public int life;
        public int armor;
        [Header("Range")]
        public float detectionRange;
        public float attackDistance;
        [Header("Speed")]
        public float rotationSpeed;
        public float attackMovementSpeed;
        public float speed;
        [Header("Timer")]
        public float hurtDuration;
        public float attackDuration;
    }

    /// <summary>
    /// 
    /// </summary>
    public class EnemyCarac : MonoBehaviour
    {

        private static EnemyCarac _instance;

        [SerializeField]
        private List<EnemySpe> listEnemy = new List<EnemySpe>();

        /// <summary>
        /// instance unique de la classe     
        /// </summary>
        public static EnemyCarac instance
        {
            get
            {
                return _instance;
            }
        }

        protected void Awake()
        {
            if (_instance != null)
            {
                throw new Exception("Tentative de création d'une autre instance de EnemyCarac alors que c'est un singleton.");
            }
            _instance = this;
        }

        public EnemySpe getEnemy(string pName)
        {
            foreach (EnemySpe lEnemy in listEnemy)
            {
                if (lEnemy.EnemyName == pName) return lEnemy;
            }
            return null;
        }

        protected void OnDestroy()
        {
            _instance = null;
        }
    }
}