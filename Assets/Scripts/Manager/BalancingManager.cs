using Assets.Scripts.CharactersScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.Manager
{

    /// <summary>
    /// 
    /// </summary>
    /// 
    [System.Serializable]
    public class PlayerCarac
    {
        [Header("Speeds")]
        public int maxJumpSpeed;
        public int maxInAirSpeed;
        public int rollSpeed;
        public int walkSpeed;
        public int runSpeed;
    }
    public class BalancingManager : BaseManager<BalancingManager>
    {
        [Header("Player")]
        [SerializeField]
        private PlayerCarac _player = new PlayerCarac();
        [Header("Enemy")]
        [SerializeField]
        private List<EnemyTemplate> listEnemy = new List<EnemyTemplate>();
        

        public PlayerCarac player
        {
            get { return _player; }
        }

        public EnemyTemplate getEnemy(string pName)
        {
            foreach (EnemyTemplate lEnemy in listEnemy)
            {
                if (lEnemy.EnemyName == pName) return lEnemy;
            }
            return null;
        }
    }
}