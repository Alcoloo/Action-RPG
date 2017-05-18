using Assets.Scripts.CharactersScripts;
using UnityEngine;
using System;
using System.Collections.Generic;
using Rpg.Characters;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tactical.Tasks;
using BehaviorDesigner.Runtime.Formations.Tasks;
using System.Collections;

namespace Rpg
{
    public class EnemyManager : BaseManager<EnemyManager> {

        private static EnemyManager _instance;

        public float spawnerCooldown = 45f;
        public int maxEnemyForStartSpawning = 5;

    public List<Transform> ennemyNear = new List<Transform>();
        private Dictionary<String, List<GameObject>> enemies = new Dictionary<String, List<GameObject>>();

        // <summary>
        /// instance unique de la classe     
        /// </summary>
        public static EnemyManager instance
        {
            get
            {
                return _instance;
            }
        }

        protected override void Awake()
        {
            base.Awake();
        }

        protected override IEnumerator CoroutineStart()
        {
            throw new NotImplementedException();
        }

        void Start()
        {
            SpawnerEnemyPopCorn.onSpawn += InitEnemy;
        }

        private GameObject popCornLead;

        #region initialisation des ennemis
        private void InitEnemy(string pName, Vector3 pPosition, bool isLead)
        {
            GameObject lEnemy = PoolingManager.manager.getFromPool(pName);
            lEnemy.SetActive(true);
            lEnemy.transform.position = pPosition;
            if (isLead) popCornLead = lEnemy;
            lEnemy.GetComponent<BehaviorTree>().EnableBehavior();
            lEnemy.GetComponent<Enemy>().init();
            //Debug.Log(lEnemy.GetComponent<BehaviorTree>().FindTask<Skirmisher>().
            pushEnemy(lEnemy);
        }

        public void testBehavior()
        {
            foreach (GameObject lEnemy in enemies["EnemyPopCorn"])
            {
                if (popCornLead != null) lEnemy.GetComponent<BehaviorTree>().FindTask<Skirmisher>().leader = popCornLead;
                lEnemy.GetComponent<BehaviorTree>().FindTask<Skirmisher>().targetTransform = lEnemy.GetComponent<EnemyPopCorn>().player.transform;
            }
        }
        #endregion

        #region Gestion des tableaux d'ennemis
        private void pushEnemy(GameObject pEnemy)
        {
            if (enemies.ContainsKey(pEnemy.name)) enemies[pEnemy.name].Add(pEnemy);
            else
            {
                List<GameObject> lList = new List<GameObject>();
                lList.Add(pEnemy);
                enemies.Add(pEnemy.name, lList);
            }
        }

        private void removeEnemy(GameObject pEnemy)
        {
            if (enemies[pEnemy.name] != null) enemies[pEnemy.name].Remove(pEnemy);
            pEnemy.SetActive(false);
        }
        #endregion

        #region gestion des spawner
        public void startSpawners()
        {
            foreach (SpawnerEnemyPopCorn lSpawner in SpawnerEnemyPopCorn.spawnerList) lSpawner.startWave();
        }

        public bool IsNotTooMuchPopCorn()
        {
            return enemies["EnemyPopCorn"].Count < maxEnemyForStartSpawning;
        }
        //startspawner gerer le temps et renvoyé success ou faillur

        public int getPopCornCount()
        {
            return enemies["EnemyPopCorn"].Count;
        }
        #endregion
    }
}
