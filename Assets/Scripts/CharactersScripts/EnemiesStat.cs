using UnityEngine;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.CharactersScripts
{

    public class EnemiesStat : MonoBehaviour
    {

        private static EnemiesStat _instance;

        [SerializeField]
        private List<EnemyTemplate> listEnemy = new List<EnemyTemplate>();

        /// <summary>
        /// instance unique de la classe     
        /// </summary>
        public static EnemiesStat instance
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
                throw new Exception("Tentative de création d'une autre instance de EnemiesStat alors que c'est un singleton.");
            }
            _instance = this;
        }

        public EnemyTemplate getEnemy(string pName)
        {
            foreach (EnemyTemplate lEnemy in listEnemy)
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