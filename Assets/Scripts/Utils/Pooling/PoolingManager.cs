using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class PoolObject
{
    public GameObject poolObject;
    public int poolSize;
}
namespace Rpg
{
    public class PoolingManager : BaseManager<PoolingManager> {

        [SerializeField]
        public List<PoolObject> poolObjectList;

        public static IDictionary<string, List<GameObject>> pool = new Dictionary<string, List<GameObject>>();


        #region initialisation
        private static PoolingManager _instance;

        /// <summary>
        /// instance unique de la classe     
        /// </summary>
        public static PoolingManager instance
        {
            get
            {
                return _instance;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            Debug.Log("awake pool " + pool.Count);
            if (!IsDestroyed && pool.Count <= 0)
            {
                Debug.Log(" INIT POOL");
                initPool();
            }
        }      

        /// <summary>
        /// initalise la pool, paramètrer les cas particuliers en fonction du nombre d'objets
        /// </summary>
        protected void initPool()
        {
            for (int i = 0; i < poolObjectList.Count; i++)
            {
                if(poolObjectList[i].poolObject != null && poolObjectList[i].poolSize > 0)
                    createObjectPool(poolObjectList[i].poolObject, poolObjectList[i].poolSize);
            }
        }

        #endregion

        #region pool creation et modification
        /// <summary>
        /// crée la pool d'un GameObject
        /// </summary>
        /// <param name="pPoolObject">GameObject</param>
        /// <param name="pPoolLength">taille de la pool</param>
        protected void createObjectPool(GameObject pPoolObject, int pPoolLength)
        {
            List<GameObject> lList = new List<GameObject>();
            string lName = pPoolObject.name;
            for (int j = 0; j < pPoolLength; j++)
            {
                instanciatePoolObject(pPoolObject, lList);
            }
            pool.Add(lName, lList);
        }

        /// <summary>
        /// instancie un GameObject et le range dans sa pool
        /// </summary>
        /// <param name="pPoolObject">GameObject a instancier</param>
        /// <param name="pPool">pool du GameObject </param>
        protected GameObject instanciatePoolObject(GameObject pPoolObject, List<GameObject> pPool)
        {
            GameObject lObject = (GameObject)Instantiate(pPoolObject);
            lObject.name = pPoolObject.name;
            lObject.SetActive(false);
            pPool.Add(lObject);
            return lObject;
        }

        #endregion

        #region pooling
        /// <summary>
        /// récupère un GameObject dans la pool
        /// </summary>
        /// <param name="pName">nom du GameObject</param>
        /// <returns>l'instance du GameObject</returns>
        public GameObject getFromPool(string pName)
        {
            bool canGetFromPool = false;
            if (pool != null)
            {
                foreach (KeyValuePair<string, List<GameObject>> entry in pool)
                {
                    if (entry.Key == pName)
                    {
                        for (int i = 0; i < entry.Value.Count; i++)
                        {
                            if (!entry.Value[i].activeInHierarchy)
                            {
                                canGetFromPool = true;
                                return entry.Value[i];
                            }
                        }
                        if (!canGetFromPool)
                        {
                            for (int j = 0; j < poolObjectList.Count; j++)
                            {
                                if (poolObjectList[j].poolObject.name == pName)
                                {
                                    GameObject lObject = instanciatePoolObject(poolObjectList[j].poolObject, entry.Value);
                                    return lObject;
                                }
                            }
                        }
                    }
                }
            } 
            return null;
        }

        public void resetPool()
        {
            Debug.Log("resetPool");
            foreach (KeyValuePair<string, List<GameObject>> entry in pool)
            {
                if (entry.Value != null)
                {
                    for (int i = 0; i < entry.Value.Count; i++)
                    {
                        if (entry.Value[i] != null && entry.Value[i].activeInHierarchy) entry.Value[i].SetActive(false);
                    }
                }
            }
        }    
        #endregion
    }
}
